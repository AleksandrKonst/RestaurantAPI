using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.Data.DTO;
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

    const int PAGE_SIZE = 5;
    
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListDishes().Skip(index).Take(count)
            .Select(v => v.DishToResource());
        var total = _db.CountDishes();
        var _links = Hal.PaginateAsDynamic("/api/dishes", index, count, total);
        var result = new
        {
            _links,
            count,
            total,
            index,
            items
        };
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Produces("application/hal+json")]
    public IActionResult Get(string id)
    {
        var dish = _db.FindDish(id);
        if (dish == default) return NotFound();
        var resource = dish.DishToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/dishes/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            },
            update = new 
            {
                href = $"/api/dishes/{id}",
                method = "PUT",
                name = $"Update {id} from the database"
            },
            create = new 
            {
                href = $"/api/dishes/{id}",
                method = "POST",
                name = $"New dish"
            }
        };
        return Ok(resource);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] DishDTO dto)
    {
        var orderItems = _db.FindOrderItemsByDish(dto.Code);
        var dish = new Dish
        {
            Code = id,
            Name = dto.Name,
            Diameter = dto.Diameter,
            Detaills = dto.Detaills,
            OrderItems = orderItems.ToList()
        };
        _db.UpdateDish(dish);
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] DishDTO dto)
    {
        var existing = _db.FindDish(dto.Code);
        if (existing != default)
            return Conflict($"Sorry, there is already a dish with this code {dto.Code} in the database.");
        var orderItems = _db.FindOrderItemsByDish(dto.Code);
        var dish = new Dish
        {
            Code = dto.Code,
            Name = dto.Name,
            Diameter = dto.Diameter,
            Detaills = dto.Detaills,
            OrderItems = orderItems.ToList()
        };
        _db.UpdateDish(dish);
        return Created($"/api/dishes/{dish.Code}", dish.DishToResource());
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var dish = _db.FindDish(id);
        if (dish == default) return NotFound();
        _db.DeleteDish(dish);
        return NoContent();
    }
}