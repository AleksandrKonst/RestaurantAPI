using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.GraphTypes;

public class OrderItemsGraphType : ObjectGraphType<OrderItem>
{
    public OrderItemsGraphType() {
        Name = "orderItem";
        Field(c => c.Order, type: typeof(OrderGraphType)).Description("Order of order");
        Field(c => c.Dish, type: typeof(DishGraphType)).Description("Dish of order");
        Field(c => c.Quantity).Description("Quantity Dish of order");
    }
}