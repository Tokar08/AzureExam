using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;


namespace CarsAzureExam.Models;

public class Car
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    [Required]
    [Range(1900, 2100)]
    public int Year { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    public string? ImageUrl { get; set; } 

    [NotMapped]
    public IFormFile? ImageFile { get; set; } 

    [SwaggerSchema(ReadOnly = true)]
    [JsonIgnore]
    public bool IsActive { get; set; } = true;
}