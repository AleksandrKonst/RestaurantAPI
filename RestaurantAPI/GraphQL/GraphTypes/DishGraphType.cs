using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.GraphTypes;

public class DishGraphType : ObjectGraphType<Dish>
{
    public DishGraphType() {
        Name = "dish";
        Field(c => c.Name).Description("The name of Dish");
        Field(c => c.Detaills).Description("The Detaills");
        Field(c => c.Diameter).Description("The Diameter");
    }
}