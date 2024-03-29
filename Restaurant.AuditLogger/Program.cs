﻿using Restaurant.Messages;
using EasyNetQ;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Restaurant.ClientServer;

namespace Restaurant.AuditLogger {
    internal class Program {
        const string SIGNALR_HUB_URL = "https://localhost:7269/hub";
        private static HubConnection hub;
        private static readonly IConfigurationRoot config = ReadConfiguration();

        static async Task Main(string[] args) {
            Console.WriteLine("Starting Restaurant.AuditLog");
            var amqp = config.GetConnectionString("RestaurantRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newClientMessages");
            var subscriberId = $"Restaurant.AuditLog@{Environment.MachineName}";
            hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
            await hub.StartAsync();
            await bus.PubSub.SubscribeAsync<NewClientMessage>(subscriberId, HandleNewClientMessage);
            Console.ReadLine();
        }

        private static async Task HandleNewClientMessage(NewClientMessage nvm) {
            using var channel = GrpcChannel.ForAddress("https://localhost:7134");
            var grpcClient = new Clienter.ClienterClient(channel);
            
            var request = new ClientRequest() {
                Code = nvm.Code,
                Name = nvm.Name,
                Number = nvm.Number,
            };
            var reply = await grpcClient.GetClientAsync(request);
            
            var csvRow =
                $"{nvm.Code},{nvm.Name},{nvm.Number},{nvm.CreatedAt:O},{reply.Check}";
            
            await hub.SendAsync("NotifyWebUsers", "Auto.Notifier", csvRow);
            
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