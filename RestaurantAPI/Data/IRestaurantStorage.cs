using RestaurantAPI.Models;

namespace RestaurantAPI.Data;

public interface IRestaurantStorage
{
    public int CountOrders();
    public IEnumerable<OrderItem> ListOrderItems();
    public IEnumerable<Order?> ListOrders();
    public IEnumerable<Client?> ListClients();
    public IEnumerable<Dish?> ListDishes();

    public IEnumerable<OrderItem> FindOrderItems(string code);
    public Order? FindOrder(string code);
    public Client? FindClient(string code);
    public Dish? FindDish(string code);
    
    public void CreateOrder(Order order);
    public void UpdateOrder(Order order);
    public void DeleteOrder(Order order);
}