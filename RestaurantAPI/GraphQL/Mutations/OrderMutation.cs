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
        
        Field<OrderGraphType>(
            "createClient",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "number"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var name = context.GetArgument<string>("name");
                var number = context.GetArgument<string>("number");
                
                var orders = db.FindOrderByClient(code);
                var client = new Client
                {
                    Code = code,
                    Name = name,
                    Number = number,
                    Orders = orders.ToList(),
                };
                _db.CreateClient(client);
                return client;
            }
        );
        
        Field<OrderGraphType>(
            "createDish",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "diameter"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "detaills"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var name = context.GetArgument<string>("name");
                var diameter = context.GetArgument<int>("diameter");
                var detaills = context.GetArgument<string>("detaills");

                var orderItems = db.FindOrderItemsByDish(code);
                var dish = new Dish
                {
                    Code = code,
                    Name = name,
                    Diameter = diameter,
                    Detaills = detaills,
                    OrderItems = orderItems.ToList()
                };
                _db.CreateDish(dish);
                return dish;
            }
        );
    }
}