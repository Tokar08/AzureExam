using CarsAzureExam.Models;

namespace CarsAzureExam.Interfaces;

public interface ICarRepository
{
    Task<Car> CreateAsync(Car car);
    Task<Car?> GetByIdAsync(int id);
    Task<IEnumerable<Car>> GetAllAsync();
    Task UpdateAsync(Car car);
    Task DeleteAsync(int id);
}