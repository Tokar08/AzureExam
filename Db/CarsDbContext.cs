using CarsAzureExam.Models;
using Microsoft.EntityFrameworkCore;

namespace CarsAzureExam.Db;

public class CarsDbContext : DbContext
{
    public CarsDbContext(DbContextOptions<CarsDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Car> Cars { get; set; }
}