using Grpc.Core;

namespace Restaurant.ClientServer.Services;

public class ClientService : Clienter.ClienterBase {
    private readonly ILogger<ClientService> _logger;

    public ClientService(ILogger<ClientService> logger)
    {
        _logger = logger;
    }
    
    public override Task<ClientReply> GetClient(ClientRequest request, ServerCallContext context) {
        return Task.FromResult(new ClientReply() { Check = true});
    }
}