using System.Text.Json.Serialization;

namespace RestaurantAPI.Models;

public class Dish
{
    public string Code { get; set; }
    public string Name { get; set; }
    public int Diameter { get; set; }
    public string Detaills { get; set; }

    [JsonIgnore]
    public ICollection<OrderItem?> OrderItems { get; set; }
}