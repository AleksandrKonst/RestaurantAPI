using RestaurantAPI.Models;

namespace RestaurantAPI.Data;

public interface IRestaurantStorage
{
    public int CountOrders();
    public int CountDishes();
    public int CountClients();
    public int CountOrderItems();
    public IEnumerable<OrderItem?> ListOrderItems();
    public IEnumerable<Order?> ListOrders();
    public IEnumerable<Client?> ListClients();
    public IEnumerable<Dish?> ListDishes();

    public OrderItem? FindOrderItems(string orderId, string dishId);
    public IEnumerable<OrderItem?> FindOrderItemsByOrder(string code);
    public IEnumerable<OrderItem?> FindOrderItemsByDish(string code);
    public Order? FindOrder(string code);
    public IEnumerable<Order> FindOrderByClient(string code);
    public Client? FindClient(string code);
    public Dish? FindDish(string code);
    
    public void CreateOrder(Order order);
    public void UpdateOrder(Order order);
    public void DeleteOrder(Order order);
    
    public void CreateDish(Dish dish);
    public void UpdateDish(Dish dish);
    public void DeleteDish(Dish dish);
    
    public void CreateClient(Client client);
    public void UpdateClient(Client client);
    public void DeleteClient(Client client);
    
    public void CreateOrderItem(OrderItem orderItem);
    public void UpdateOrderItem(OrderItem orderItem);
    public void DeleteOrderItem(OrderItem orderItem);
}