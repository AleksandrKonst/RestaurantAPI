using EasyNetQ;
using Microsoft.Extensions.Configuration;

namespace Restaurant.AuditLogger {
    internal class Program {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static async Task Main(string[] args) {
            Console.WriteLine("Starting Restaurant.AuditLogger");
            var amqp = config.GetConnectionString("RestaurantRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newClientMessages");
            var subscriberId = $"Restaurant.AuditLogger@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewClientMessage>(subscriberId, HandleNewClientMessage);
            Console.ReadLine();
        }

        private static void HandleNewClientMessage(NewClientMessage nvm) {
            var csvRow =
                $"{nvm.Code},{nvm.Name},{nvm.Number},{nvm.CreatedAt:O}";
            Console.WriteLine(csvRow);
        }

        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}