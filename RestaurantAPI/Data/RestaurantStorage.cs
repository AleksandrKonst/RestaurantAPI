using RestaurantAPI.Models;

namespace RestaurantAPI.Data;

public class RestaurantStorage : IRestaurantStorage
{
    private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;
    
    private readonly Dictionary<string, Dish?> dishes = new Dictionary<string, Dish?>(collation);
    private readonly Dictionary<string, Client?> clients = new Dictionary<string, Client?>(collation);
    private readonly Dictionary<string, Order?> orders = new Dictionary<string, Order?>(collation);
    private readonly List<OrderItem> orderItems = new List<OrderItem>();
    private readonly ILogger<RestaurantStorage> logger;
    
    
    public RestaurantStorage(ILogger<RestaurantStorage> logger) {
        this.logger = logger;
        ReadDishesFromCsvFile("dish.csv");
        ReadClientsFromCsvFile("client.csv");
        ReadOrdersFromCsvFile("order.csv");
        ReadOrderItemsFromCsvFile("order-items.csv");
        ResolveReferences();
    }
    
    private string ResolveCsvFilePath(string filename) {
        return Path.Combine("Data\\csv-data\\", filename);
    }
    
    private void ResolveReferences()
    {
        foreach (var dish in dishes.Values) {
            dish.OrderItems = orderItems.Where(order => order.DishCode == dish.Code).ToList();
            foreach (var orderItem in dish.OrderItems) orderItem.Dish = dish;
        }

        foreach (var client in clients.Values) {
            client.Orders = orders.Values.Where(order => order.ClientCode == client.Code).ToList();
            foreach (var order in client.Orders) order.Client = client;
        }
        
        foreach (var order in orders.Values) {
            order.OrderItems = orderItems.Where(orderItem => orderItem.OrderCode == order.Code).ToList();
            foreach (var orderItem in order.OrderItems) orderItem.Order = order;
        }
    }
    
    private void ReadDishesFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath)) {
            var tokens = line.Split(",");
            var dish = new Dish {
                Code = tokens[0],
                Name = tokens[1],
                Diameter = Int32.Parse(tokens[2]),
                Detaills = tokens[3]
            };
            dishes.Add(dish.Code, dish);
        }
        logger.LogInformation($"Loaded {dishes.Count} dishes from {filePath}");
    }

    private void ReadClientsFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath)) {
            var tokens = line.Split(",");
            var client = new Client {
                Code = tokens[0],
                Name = tokens[1],
                Number = tokens[2]
            };
            clients.Add(client.Code, client);
        }
        logger.LogInformation($"Loaded {clients.Count} clients from {filePath}");
    }

    private void ReadOrdersFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath)) {
            var tokens = line.Split(",");
            var order = new Order {
                Code = tokens[0],
                ClientCode = tokens[1],
                Address = tokens[2]
            };
            orders.Add(order.Code, order);
        }
        logger.LogInformation($"Loaded {orders.Count} orders from {filePath}");
    }

    private void ReadOrderItemsFromCsvFile(string filename)
    {
        var filePath = ResolveCsvFilePath(filename);
        foreach (var line in File.ReadAllLines(filePath)) {
            var tokens = line.Split(",");
            var orderItem = new OrderItem {
                OrderCode = tokens[0],
                DishCode = tokens[1],
                Quantity = tokens[2]
            };
            orderItems.Add(orderItem);
        }
        logger.LogInformation($"Loaded {orderItems.Count} orderItems from {filePath}");
    }

    public int CountOrders() => orders.Count;
    
    public int CountDishes() => dishes.Count;
    
    public int CountClients() => clients.Count;
    
    public int CountOrderItems() => orderItems.Count;

    public IEnumerable<OrderItem> ListOrderItems() => orderItems;

    public IEnumerable<Order?> ListOrders() => orders.Values;

    public IEnumerable<Client?> ListClients() => clients.Values;

    public IEnumerable<Dish?> ListDishes() => dishes.Values;

    public IEnumerable<OrderItem> FindOrderItems(string code) => orderItems.Where(order => order.OrderCode == code);
    
    public IEnumerable<OrderItem> FindOrderItemsByDish(string code) => orderItems.Where(order => order.DishCode == code);

    public Order? FindOrder(string code) => orders.GetValueOrDefault(code);

    public Client? FindClient(string code) => clients.GetValueOrDefault(code);

    public Dish? FindDish(string code) => dishes.GetValueOrDefault(code);

    public void CreateOrder(Order order)
    {
        order.Client.Orders.Add(order);
        order.ClientCode = order.Client.Code;
        UpdateOrder(order);
    }

    public void UpdateOrder(Order order)
    {
        orders[order.Code] = order;
    }

    public void DeleteOrder(Order order)
    {
        var model = FindClient(order.ClientCode);
        model.Orders.Remove(order);
        orders.Remove(order.Code);
    }

    public void CreateDish(Dish dish)
    {
        UpdateDish(dish);
    }

    public void UpdateDish(Dish dish)
    {
        dishes[dish.Code] = dish;
    }

    public void DeleteDish(Dish dish)
    {
        orderItems.RemoveAll(item => item.DishCode == dish.Code);
        dishes.Remove(dish.Code);
    }

    public void CreateClient(Client client)
    {
        throw new NotImplementedException();
    }

    public void UpdateClient(Client client)
    {
        throw new NotImplementedException();
    }

    public void DeleteClient(Client client)
    {
        throw new NotImplementedException();
    }

    public void CreateOrderItem(OrderItem orderItem)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrderItem(OrderItem orderItem)
    {
        throw new NotImplementedException();
    }

    public void DeleteOrderItem(OrderItem orderItem)
    {
        throw new NotImplementedException();
    }
}