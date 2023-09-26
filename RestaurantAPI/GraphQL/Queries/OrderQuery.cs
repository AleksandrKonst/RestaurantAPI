using GraphQL;
using GraphQL.Types;
using RestaurantAPI.Data;
using RestaurantAPI.GraphQL.GraphTypes;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.Queries;

public class OrderQuery : ObjectGraphType
{
    private readonly IRestaurantStorage _db;

    public OrderQuery(IRestaurantStorage db) {
        this._db = db;

        Field<ListGraphType<OrderGraphType>>("Orders", "Query to retrieve all orders",
            resolve: GetAllOrders);

        Field<OrderGraphType>("Order", "Query to retrieve a specific order",
            new QueryArguments(MakeNonNullStringArgument("code", "The code of the order")),
            resolve: GetOrder);

        Field<ListGraphType<OrderGraphType>>("OrdersByClient", "Query to retrieve all order matching the specified client",
            new QueryArguments(MakeNonNullStringArgument("client", "The code of a client 'AA47'")),
            resolve: GetOrdersByClient);
        
        Field<ListGraphType<ClientGraphType>>("Clients", "Client to retrieve all clients",
            resolve: GetAllClients);

        Field<ClientGraphType>("Client", "Query to retrieve a specific client",
            new QueryArguments(MakeNonNullStringArgument("code", "The code of the client")),
            resolve: GetClient);

        Field<ListGraphType<DishGraphType>>("Dishes", "Query to retrieve all dishes",
            resolve: GetAllDishes);

        Field<DishGraphType>("Dish", "Query to retrieve a specific dish",
            new QueryArguments(MakeNonNullStringArgument("code", "The code of the dish")),
            resolve: GetDish);
        
        
        Field<ListGraphType<OrderItemsGraphType>>("OrderItems", "Query to retrieve all orderItems",
            resolve: GetAllOrderItems);

        Field<OrderItemsGraphType>("OrderItem", "Query to retrieve a specific orderItem",
            new QueryArguments(MakeNonNullStringArgument("codeOrder", "The code of the orderItem"), MakeNonNullStringArgument("codeDish", "The code of the orderItem")),
            resolve: GetOrderItems);
    }

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }

    private IEnumerable<Order> GetAllOrders(IResolveFieldContext<object> context) => _db.ListOrders();

    private Order GetOrder(IResolveFieldContext<object> context) {
        var code = context.GetArgument<string>("code");
        return _db.FindOrder(code);
    }

    private IEnumerable<Order> GetOrdersByClient(IResolveFieldContext<object> context) {
        var clientCode = context.GetArgument<string>("client");
        var orders = _db.ListOrders().Where(o => o.ClientCode == clientCode);
        return orders;
    }
    
    private IEnumerable<Client> GetAllClients(IResolveFieldContext<object> context) => _db.ListClients();

    private Client GetClient(IResolveFieldContext<object> context) {
        var code = context.GetArgument<string>("code");
        return _db.FindClient(code);
    }
    
    private IEnumerable<Dish> GetAllDishes(IResolveFieldContext<object> context) => _db.ListDishes();

    private Dish GetDish(IResolveFieldContext<object> context) {
        var code = context.GetArgument<string>("code");
        return _db.FindDish(code);
    }
    
    private IEnumerable<OrderItem> GetAllOrderItems(IResolveFieldContext<object> context) => _db.ListOrderItems();

    private OrderItem GetOrderItems(IResolveFieldContext<object> context) {
        var codeOrder = context.GetArgument<string>("codeOrder");
        var codeDish = context.GetArgument<string>("codeDish");
        return _db.FindOrderItems(codeOrder, codeDish);
    }
}