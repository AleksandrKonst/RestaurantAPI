using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.Data.DTO;
using RestaurantAPI.HAL;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers.RESTful;

[Route("api/order-items")]
[ApiController]
public class OrderItemsController : ControllerBase
{
    private readonly IRestaurantStorage _db;
    
    public OrderItemsController(IRestaurantStorage db)
    {
        this._db = db;
    }
    
    const int PAGE_SIZE = 10;
    
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListOrderItems().Skip(index).Take(count)
            .Select(v => v.OrderItemsToResource());
        var total = _db.CountOrderItems();
        var _links = Hal.PaginateAsDynamic("/api/order-items", index, count, total);
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

    [HttpGet("{orderId}/{dishId}")]
    [Produces("application/hal+json")]
    public IActionResult Get(string orderId, string dishId)
    {
        var order = _db.FindOrderItems(orderId, dishId);
        if (order == default) return NotFound();
        var resource = order.OrderItemsToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/order-items/{orderId}/{dishId}",
                method = "DELETE",
                name = $"Delete {orderId}/{dishId} from the database"
            },
            update = new 
            {
                href = $"/api/order-items/{orderId}/{dishId}",
                method = "PUT",
                name = $"Update {orderId}/{dishId} from the database"
            },
            create = new 
            {
                href = $"/api/order-items/{orderId}/{dishId}",
                method = "POST",
                name = $"New orderItems"
            }
        };
        return Ok(resource);
    }
    
    [HttpGet("order/{orderId}")]
    [Produces("application/hal+json")]
    public IActionResult GetByOrder(string orderId, int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListOrderItems().Where(order => order.OrderCode == orderId).Skip(index).Take(count)
            .Select(v => v.OrderItemsToResource());
        var total = items.Count();
        var _links = Hal.PaginateAsDynamic("/api/order-items", index, count, total);
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
    
    [HttpGet("dish/{dishId}")]
    [Produces("application/hal+json")]
    public IActionResult GetByDish(string dishId, int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListOrderItems().Where(order => order.DishCode == dishId).Skip(index).Take(count)
            .Select(v => v.OrderItemsToResource());
        var total = items.Count();
        var _links = Hal.PaginateAsDynamic("/api/order-items", index, count, total);
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
    
    [HttpPut("{orderId}/{dishId}")]
    public IActionResult Put(string orderId, string dishId, [FromBody] OrderItemsDTO dto)
    {
        var client = _db.FindOrderItems(orderId, dishId);
        var order = _db.FindOrder(orderId);
        var dish = _db.FindDish(dishId);
        var orderItems = new OrderItem
        {
            OrderCode = orderId,
            Order = order,
            DishCode = dishId,
            Dish = dish,
            Quantity = dto.Quantity
        };
        _db.UpdateOrderItem(orderItems);
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderItemsDTO dto)
    {
        var existing = _db.FindOrderItems(dto.OrderCode, dto.DishCode);
        if (existing != default)
            return Conflict($"Sorry, there is already a orderItem with this code {dto.OrderCode} {dto.DishCode}  in the database.");
        var order = _db.FindOrder(dto.OrderCode);
        var dish = _db.FindDish(dto.DishCode);
        var orderItems = new OrderItem
        {
            OrderCode = dto.OrderCode,
            Order = order,
            DishCode = dto.DishCode,
            Dish = dish,
            Quantity = dto.Quantity
        };
        _db.CreateOrderItem(orderItems);
        return Created($"/api/order-items/{orderItems.OrderCode}/{orderItems.DishCode}", orderItems.OrderItemsToResource());
    }
    
    [HttpDelete("{orderId}/{dishId}")]
    public IActionResult Delete(string orderId, string dishId)
    {
        var order = _db.FindOrderItems(orderId, dishId);
        if (order == default) return NotFound();
        _db.DeleteOrderItem(order);
        return NoContent();
    }
}