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
}