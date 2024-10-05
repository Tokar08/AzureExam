using CarsAzureExam.Models;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly CarService _carService;

    public CarsController(CarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCars()
    {
        var cars = await _carService.GetAllCarsAsync();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Car>> GetCarById(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    [HttpPost]
    public async Task<ActionResult<Car>> AddCar([FromForm] Car car)
    {
        var addedCar = await _carService.AddCarAsync(car);
        return CreatedAtAction(nameof(GetCarById), new { id = addedCar.Id }, addedCar);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, [FromForm] Car car)
    {
        if (id != car.Id)
        {
            return BadRequest();
        }

        await _carService.UpdateCarAsync(car);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        await _carService.DeleteCarAsync(id);
        return NoContent();
    }
}