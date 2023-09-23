using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.GraphTypes;

public class OrderGraphType : ObjectGraphType<Order>
{
    public OrderGraphType() {
        Name = "order";
        Field(c => c.Client, type: typeof(ClientGraphType)).Description("Client");
        Field(c => c.Address).Description("Address of order");
    }
}