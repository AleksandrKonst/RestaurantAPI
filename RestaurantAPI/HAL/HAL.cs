using System.ComponentModel;
using System.Dynamic;
using RestaurantAPI.Models;

namespace RestaurantAPI.HAL;

public static class Hal
{
        public static dynamic PaginateAsDynamic(string baseUrl, int index, int count, int total) {
                dynamic links = new ExpandoObject();
                links.self = new { href = baseUrl };
                if (index < total) {
                    links.next = new { href = $"{baseUrl}?index={index + count}" };
                    links.final = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
                }
                if (index > 0) {
                    links.prev = new { href = $"{baseUrl}?index={index - count}" };
                    links.first = new { href = $"{baseUrl}?index=0" };
                }
                return links;
        }

        public static dynamic OrderToResource(this Order order) {
                var resource = order.ToDynamic();
                resource._links = new {
                    self = new {
                        href = $"/api/orders/{order.Code}"
                    },
                    client = new {
                        href = $"/api/clients/{order.ClientCode}"
                    },
                    orderItems = new {
                        href = $"/api/order-items/?orderCode={order.Code}"
                    }
                };
                return resource;
        }
        
        public static dynamic DishToResource(this Dish dish) {
            var resource = dish.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/dishes/{dish.Code}"
                },
                orderItems = new {
                    href = $"/api/order-items?dishCode={dish.Code}"
                }
            };
            return resource;
        }
        
        public static dynamic OrderItemsToResource(this Order order) {
            var resource = order.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/orders/{order.Code}"
                },
                client = new {
                    href = $"/api/clients/{order.ClientCode}"
                },
                orderItems = new {
                    href = $"/api/order-items/{order.Code}"
                }
            };
            return resource;
        }
        
        public static dynamic ClientToResource(this Order order) {
            var resource = order.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/orders/{order.Code}"
                },
                client = new {
                    href = $"/api/clients/{order.ClientCode}"
                },
                orderItems = new {
                    href = $"/api/order-items/{order.Code}"
                }
            };
            return resource;
        }

        public static dynamic ToDynamic(this object value) {
            IDictionary<string, object> result = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor prop in properties) {
                if (Ignore(prop)) continue;
                result.Add(prop.Name, prop.GetValue(value));
            }
            return result;
        }

        private static bool Ignore(PropertyDescriptor prop) {
            return prop.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any();
        }
}