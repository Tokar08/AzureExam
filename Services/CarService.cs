using Microsoft.Extensions.Caching.Distributed;
using CarsAzureExam.Interfaces;
using CarsAzureExam.Models;
using Newtonsoft.Json;

public class CarService
{
    private readonly ICarRepository _carRepository;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<CarService> _logger;

    public CarService(ICarRepository carRepository, IBlobStorageService blobStorageService, IDistributedCache cache, ILogger<CarService> logger)
    {
        _carRepository = carRepository;
        _blobStorageService = blobStorageService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Car> AddCarAsync(Car car)
    {
        try
        {
            if (car.ImageFile != null)
            {
                car.ImageUrl = await _blobStorageService.UploadFileAsync(car.ImageFile, "car-images");
            }

            var addedCar = await _carRepository.CreateAsync(car);
            
            await _cache.RemoveAsync("all-cars");
            await _cache.SetStringAsync($"car-{addedCar.Id}", JsonConvert.SerializeObject(addedCar));

            return addedCar;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding car");
            throw;
        }
    }

    public async Task UpdateCarAsync(Car car)
    {
        try
        {
            if (car.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(car.ImageUrl))
                {
                    await _blobStorageService.DeleteFileAsync(car.ImageUrl, "car-images");
                }

                car.ImageUrl = await _blobStorageService.UploadFileAsync(car.ImageFile, "car-images");
            }

            await _carRepository.UpdateAsync(car);
            
            await _cache.RemoveAsync("all-cars");
            await _cache.SetStringAsync($"car-{car.Id}", JsonConvert.SerializeObject(car));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating car");
            throw;
        }
    }

    public async Task DeleteCarAsync(int id)
    {
        try
        {
            await _carRepository.DeleteAsync(id);
            
            await _cache.RemoveAsync($"car-{id}");
            await _cache.RemoveAsync("all-cars");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting car!");
            throw;
        }
    }

    public async Task<Car?> GetCarByIdAsync(int id)
    {
        try
        {
            var cachedCar = await _cache.GetStringAsync($"car-{id}");
            if (cachedCar != null)
            {
                return JsonConvert.DeserializeObject<Car>(cachedCar);
            }

            var car = await _carRepository.GetByIdAsync(id);
            if (car != null)
            {
                await _cache.SetStringAsync($"car-{car.Id}", JsonConvert.SerializeObject(car));
            }

            return car;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching car by id!");
            throw;
        }
    }

    public async Task<IEnumerable<Car>> GetAllCarsAsync()
    {
        try
        {
            var cachedCars = await _cache.GetStringAsync("all-cars");
            if (cachedCars != null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Car>>(cachedCars)!;
            }

            var cars = await _carRepository.GetAllAsync();
            if (!cars.Any()) 
                return cars;
            
            var carsJson = JsonConvert.SerializeObject(cars);
            await _cache.SetStringAsync("all-cars", carsJson, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            });

            return cars;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching all cars!");
            throw;
        }
    }
}
