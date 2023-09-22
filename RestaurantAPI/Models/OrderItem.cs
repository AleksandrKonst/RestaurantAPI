using System.Text.Json.Serialization;

namespace RestaurantAPI.Models;

public class OrderItem
{
    public string OrderCode { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
    public string DishCode { get; set; }
    [JsonIgnore]
    public Dish? Dish { get; set; }
    public string Quantity { get; set; }
}