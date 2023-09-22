using System.Text.Json.Serialization;

namespace RestaurantAPI.Models;

public class Order
{
    public string Code { get; set; }
    public string ClientCode { get; set; }
    [JsonIgnore]
    public Client? Client{ get; set; }
    public string Address { get; set; }
    [JsonIgnore]
    public ICollection<OrderItem?> OrderItems { get; set; }
}