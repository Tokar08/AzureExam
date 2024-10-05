using CarsAzureExam.Db;
using CarsAzureExam.Interfaces;
using CarsAzureExam.Models;
using Microsoft.EntityFrameworkCore;

namespace CarsAzureExam.Repositories;

public class CarRepository : ICarRepository
{
    private readonly CarsDbContext _context;

    public CarRepository(CarsDbContext context)
    {
        _context = context;
    }

    public async Task<Car> CreateAsync(Car car)
    {
        try
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while creating the car!", ex);
        }
    }

    public async Task<Car?> GetByIdAsync(int id)
    {
        try
        {
            return await _context.Cars.FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while fetching the car!", ex);
        }
    }

    public async Task<IEnumerable<Car>> GetAllAsync()
    {
        try
        {
            return await _context.Cars.Where(c => c.IsActive).ToListAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while fetching all cars!", ex);
        }
    }

    public async Task UpdateAsync(Car car)
    {
        try
        {
            var existingCar = await GetByIdAsync(car.Id);
            if (existingCar != null)
            {
                existingCar.Brand = car.Brand;
                existingCar.Model = car.Model;
                existingCar.Year = car.Year;
                existingCar.Price = car.Price;
                existingCar.ImageUrl = car.ImageUrl; 

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Car not found!");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while updating the car!", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var car = await GetByIdAsync(id);
            if (car != null)
            {
                car.IsActive = false; 
                await UpdateAsync(car);
            }
            else
            {
                throw new InvalidOperationException("Car not found");
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error while deleting the car.", ex);
        }
    }
}
