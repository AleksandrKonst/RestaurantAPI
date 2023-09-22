using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.HAL;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers.RESTful;

[Route("api/[controller]")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly IRestaurantStorage _db;

    public DishesController(IRestaurantStorage db)
    {
        this._db = db;
    }

    [HttpGet]
    public IEnumerable<Dish?> Get()
    {
        return _db.ListDishes();
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var dish = _db.FindDish(id);
        if (dish == default) return NotFound();
        var resource = dish.ToDynamic();
        resource._actions = new
        {
            create = new
            {
                href = $"/api/dish/{id}",
                type = "application/json",
                method = "POST",
                name = $"Create a new dish"
            }
        };
        return Ok(resource);
    }
}