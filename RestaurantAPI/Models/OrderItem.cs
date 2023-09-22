namespace RestaurantAPI.Models;

public class OrderItem
{
    public string OrderCode { get; set; }
    public Order? Order { get; set; }
    public string DishCode { get; set; }
    public Dish? Dish { get; set; }
    public string Quantity { get; set; }
}