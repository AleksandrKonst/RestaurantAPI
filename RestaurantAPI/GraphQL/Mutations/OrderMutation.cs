using GraphQL;
using GraphQL.Types;
using RestaurantAPI.Data;
using RestaurantAPI.GraphQL.GraphTypes;
using RestaurantAPI.GraphQL.InputType;
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
                new QueryArgument<NonNullGraphType<OrderInputType>> { Name = "order"}
            ),
            resolve: context =>
            {
                var order = context.GetArgument<Order>("order");
                var client = db.FindClient(order.ClientCode);
                
                if (db.FindOrder(order.Code) != default) return null;
                if (client == default) return null;
                
                var orderItems = db.FindOrderItemsByOrder(order.Code);

                order.Client = client;
                order.OrderItems = orderItems.ToList();
                
                _db.CreateOrder(order);
                return order;
            }
        );
        
        Field<OrderGraphType>(
            "deleteOrder",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
               
                var order = _db.FindOrder(code);
                if (order == default) return null;
                _db.DeleteOrder(order);
                return order;
            }
        );
        
        Field<OrderGraphType>(
            "updateOrder",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<OrderInputType>> { Name = "order"}
            ),
            resolve: context =>
            {
                var order = context.GetArgument<Order>("order");
                var client = db.FindClient(order.ClientCode);
                
                if (client == default) return null;
                
                var orderItems = db.FindOrderItemsByOrder(order.Code);

                order.Client = client;
                order.OrderItems = orderItems.ToList();
                
                _db.UpdateOrder(order);
                return order;
            }
        );
        
        Field<ClientGraphType>(
            "createClient",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<ClientInputType>> { Name = "client"}
            ),
            resolve: context =>
            {
                var client = context.GetArgument<Client>("client");

                if (db.FindClient(client.Code) != default) return null;
                
                var orders = db.FindOrderByClient(client.Code);
                client.Orders = orders.ToList();
                _db.CreateClient(client);
                return client;
            }
        );
        
        Field<ClientGraphType>(
            "deleteClient",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
               
                var client = _db.FindClient(code);
                if (client == default) return null;
                _db.DeleteClient(client);
                return client;
            }
        );
        
        Field<ClientGraphType>(
            "updateClient",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<ClientInputType>> { Name = "client"}
            ),
            resolve: context =>
            {
                var client = context.GetArgument<Client>("client");

                if (db.FindClient(client.Code) == default) return null;
                
                var orders = db.FindOrderByClient(client.Code);
                client.Orders = orders.ToList();
                _db.UpdateClient(client);
                return client;
            }
        );
        
        Field<DishGraphType>(
            "createDish",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<DishInputType>> { Name = "dish"}
            ),
            resolve: context =>
            {
                var dish = context.GetArgument<Dish>("dish");
                
                if (db.FindDish(dish.Code) == default) return null;
                
                var orderItems = db.FindOrderItemsByDish(dish.Code);
                dish.OrderItems = orderItems.ToList();
                _db.CreateDish(dish);
                return dish;
            }
        );
        
        Field<DishGraphType>(
            "deleteDish",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "code"}
            ),
            resolve: context =>
            {
                var code = context.GetArgument<string>("code");
                var dish = _db.FindDish(code);
                
                if (dish == default) return null;
                _db.DeleteDish(dish);
                return dish;
            }
        );
        
        Field<DishGraphType>(
            "updateDish",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<DishInputType>> { Name = "dish"}
            ),
            resolve: context =>
            {
                var dish = context.GetArgument<Dish>("dish");
                
                if (db.FindDish(dish.Code) == default) return null;
                
                var orderItems = db.FindOrderItemsByDish(dish.Code);
                dish.OrderItems = orderItems.ToList();
                _db.UpdateDish(dish);
                return dish;
            }
        );
    }
}