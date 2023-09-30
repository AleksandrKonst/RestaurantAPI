using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.InputType;

public class OrderInputType : InputObjectGraphType<Order>
{
    public OrderInputType()
    {
        Name = "orderInput";
        Field(c => c.Code);
        Field(c => c.ClientCode);
        Field(c => c.Address);
    }
}