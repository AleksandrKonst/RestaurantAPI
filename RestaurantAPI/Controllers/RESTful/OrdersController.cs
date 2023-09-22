using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.Data.DTO;
using RestaurantAPI.HAL;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers.RESTful;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IRestaurantStorage _db;
    
    public OrdersController(IRestaurantStorage db)
    {
        this._db = db;
    }
    
    const int PAGE_SIZE = 5;
    
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListOrders().Skip(index).Take(count)
            .Select(v => v.OrderToResource());
        var total = _db.CountOrders();
        var _links = Hal.PaginateAsDynamic("/api/orders", index, count, total);
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
        var order = _db.FindOrder(id);
        if (order == default) return NotFound();
        var resource = order.OrderToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/orders/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            },
            update = new 
            {
                href = $"/api/orders/{id}",
                method = "PUT",
                name = $"Update {id} from the database"
            },
            create = new 
            {
                href = $"/api/orders/{id}",
                method = "POST",
                name = $"New order"
            }
        };
        return Ok(resource);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] OrderDTO dto)
    {
        var client = _db.FindClient(dto.ClientCode);
        var orderItems = _db.FindOrderItemsByOrder(dto.Code);
        var order = new Order
        {
            Code = dto.Code,
            ClientCode = dto.ClientCode,
            Client = client,
            Address = dto.Address,
            OrderItems = orderItems.ToList()
        };
        _db.UpdateOrder(order);
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderDTO dto)
    {
        var existing = _db.FindOrder(dto.Code);
        if (existing != default)
            return Conflict($"Sorry, there is already a order with this code {dto.Code} in the database.");
        var client = _db.FindClient(dto.ClientCode);
        var orderItems = _db.FindOrderItemsByOrder(dto.Code);
        var order = new Order
        {
            Code = dto.Code,
            ClientCode = dto.ClientCode,
            Client = client,
            Address = dto.Address,
            OrderItems = orderItems.ToList()
        };
        _db.CreateOrder(order);
        return Created($"/api/orders/{order.Code}", order.OrderToResource());
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var order = _db.FindOrder(id);
        if (order == default) return NotFound();
        _db.DeleteOrder(order);
        return NoContent();
    }
    
    [HttpGet("client/{clientId}")]
    [Produces("application/hal+json")]
    public IActionResult GetClient(string clientId, int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListOrders().Where(order => order.ClientCode == clientId).Skip(index).Take(count)
            .Select(v => v.OrderToResource());
        var total = items.Count();
        var _links = Hal.PaginateAsDynamic("/api/orders", index, count, total);
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
}