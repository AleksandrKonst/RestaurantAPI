using GraphQL.Types;
using RestaurantAPI.Models;

namespace RestaurantAPI.GraphQL.InputType;

public class DishInputType : InputObjectGraphType<Dish>
{
    public DishInputType()
    {
        Name = "dishInput";
        Field(c => c.Code);
        Field(c => c.Name);
        Field(c => c.Detaills);
        Field(c => c.Diameter);
    }
}