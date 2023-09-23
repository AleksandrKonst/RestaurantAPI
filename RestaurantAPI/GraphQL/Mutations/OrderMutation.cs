using GraphQL;
using GraphQL.Types;
using RestaurantAPI.Data;
using RestaurantAPI.GraphQL.GraphTypes;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.Mutations;

public class OrderMutation: ObjectGraphType
{
    private readonly IRestaurantStorage _db;

    public OrderMutation(IRestaurantStorage db)
    {
        this._db = db;
        
        Field<OrderGraphType>(
            "createOrder",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "address"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "clientCode"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var address = context.GetArgument<string>("address");
                var clientCode = context.GetArgument<string>("clientCode");

                var client = db.FindClient(clientCode);
                var orderItems = db.FindOrderItemsByOrder(code);
                var order = new Order
                {
                    Code = code,
                    ClientCode = clientCode,
                    Client = client,
                    Address = address,
                    OrderItems = orderItems.ToList()
                };
                _db.CreateOrder(order);
                return order;
            }
        );
    }
}