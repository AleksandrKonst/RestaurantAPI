using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.InputType;

public class ClientInputType : InputObjectGraphType<Client>
{
    public ClientInputType()
    {
        Name = "clientInput";
        Field(c => c.Code);
        Field(c => c.Name);
        Field(c => c.Number);
    }
}