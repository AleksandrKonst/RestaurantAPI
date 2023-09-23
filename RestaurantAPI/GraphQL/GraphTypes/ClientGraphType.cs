using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.GraphTypes;

public class ClientGraphType : ObjectGraphType<Client>
{
    public ClientGraphType() {
        Name = "client";
        Field(c => c.Name).Description("The name of the client");
        Field(c => c.Number).Description("The telephone number");
    }
}