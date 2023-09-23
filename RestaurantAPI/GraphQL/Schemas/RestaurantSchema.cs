using GraphQL.Types;
using RestaurantAPI.Data;
using RestaurantAPI.GraphQL.Mutations;
using RestaurantAPI.GraphQL.Queries;

namespace RestaurantAPI.GraphQL.Schemas;

public class RestaurantSchema : Schema
{
    public RestaurantSchema(IRestaurantStorage db)
    {
        Query = new OrderQuery(db);
        Mutation = new OrderMutation(db);
    }
}