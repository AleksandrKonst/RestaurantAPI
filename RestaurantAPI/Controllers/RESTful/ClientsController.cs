using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Data;
using RestaurantAPI.Data.DTO;
using RestaurantAPI.HAL;
using RestaurantAPI.Models;

namespace RestaurantAPI.Controllers.RESTful;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IRestaurantStorage _db;
    private readonly IBus _bus;

    public ClientsController(IRestaurantStorage db, IBus bus)
    {
        this._db = db;
        this._bus = bus;
    }

    const int PAGE_SIZE = 5;
    
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListClients().Skip(index).Take(count)
            .Select(v => v.ClientToResource());
        var total = _db.CountClients();
        var _links = Hal.PaginateAsDynamic("/api/clients", index, count, total);
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
        var client = _db.FindClient(id);
        if (client == default) return NotFound();
        var resource = client.ClientToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/clients/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            },
            update = new 
            {
                href = $"/api/clients/{id}",
                method = "PUT",
                name = $"Update {id} from the database"
            },
            create = new 
            {
                href = $"/api/clients/{id}",
                method = "POST",
                name = $"New client"
            }
        };
        return Ok(resource);
    }
    
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] ClientDTO dto)
    {
        var orders = _db.FindOrderByClient(dto.Code);
        var client = new Client
        {
            Code = id,
            Name = dto.Name,
            Number = dto.Number,
            Orders = orders.ToList()
        };
        _db.UpdateClient(client);
        return Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ClientDTO dto)
    {
        var existing = _db.FindClient(dto.Code);
        if (existing != default)
            return Conflict($"Sorry, there is already a client with this code {dto.Code} in the database.");
        var orders = _db.FindOrderByClient(dto.Code);
        var client = new Client
        {
            Code = dto.Code,
            Name = dto.Name,
            Number = dto.Number,
            Orders = orders.ToList()
        };
        _db.CreateClient(client);
        await PublishNewClientMessage(client);
        return Created($"/api/clients/{client.Code}", client.ClientToResource());
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var client = _db.FindClient(id);
        if (client == default) return NotFound();
        _db.DeleteClient(client);
        return NoContent();
    }
    
    private async Task PublishNewClientMessage(Client client) {
        var message = client.ToMessage();
        await _bus.PubSub.PublishAsync(message);
    }
}