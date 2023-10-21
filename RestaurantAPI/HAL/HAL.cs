using System.ComponentModel;
using System.Dynamic;
using RestaurantAPI.Models;
using RestaurantMS;

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
                        href = $"/api/order-items/order/{order.Code}"
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
                    href = $"/api/order-items/dish/{dish.Code}"
                }
            };
            return resource;
        }
        
        public static dynamic OrderItemsToResource(this OrderItem orderItem) {
            var resource = orderItem.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/order-items/{orderItem.OrderCode}/{orderItem.DishCode}"
                },
                order = new {
                    href = $"/api/orders/{orderItem.OrderCode}"
                },
                dish = new {
                    href = $"/api/dishes/{orderItem.DishCode}"
                }
            };
            return resource;
        }
        
        public static dynamic ClientToResource(this Client client) {
            var resource = client.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/clients/{client.Code}"
                },
                order = new {
                    href = $"/api/orders/client/{client.Code}"
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
        
        public static NewClientMessage ToMessage(this Client client) {
            var message = new NewClientMessage() {
                Code = client.Code,
                Name = client.Name,
                Number = client.Number,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            return message;
        }

        private static bool Ignore(PropertyDescriptor prop) {
            return prop.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any();
        }
}