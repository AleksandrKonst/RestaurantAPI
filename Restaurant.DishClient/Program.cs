using Grpc.Net.Client;
using Restaurant.DishServer;

using var channel = GrpcChannel.ForAddress("https://localhost:7184");
var grpcClient = new Disher.DisherClient(channel);
Console.WriteLine("Ready! Press any key to send a gRPC request (or Ctrl-C to quit):");
while (true) {
    Console.ReadKey(true);
    var request = new DishRequest {
        Code = "four-cheeses",
        Name = "Four Cheeses",
        Diameter = 35,
        Detaills = "моцарелла фирменный соус альфредо"
    };
    var reply = grpcClient.GetDish(request);
    Console.WriteLine($"{reply.Message}");
}