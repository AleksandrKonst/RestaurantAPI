using System.Text.Json.Serialization;

namespace RestaurantAPI.Models;

public class Client
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }

    [JsonIgnore]
    public ICollection<Order?> Orders { get; set; }
}