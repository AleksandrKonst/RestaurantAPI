using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers.RESTful;

[Route("api")]
[ApiController]
public class DiscoveryEndpointController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() {
        var welcome = new {
            _links = new {
                orders = new {
                    href = "/api/order"
                },
                models = new {
                    href = "/api/model"
                },
                clients = new {
                    href = "/api/client"
                },
                orderItems = new {
                    href = "/api/order-items"
                }
            },
            message = "Welcome to the Restaurant API!",
        };
        return Ok(welcome);
    }
}