using Grpc.Core;

namespace Restaurant.DishServer.Services;

public class DishService : Disher.DisherBase {
    private readonly ILogger<DishService> _logger;

    public DishService(ILogger<DishService> logger)
    {
        _logger = logger;
    }
    
    public override Task<DishReply> GetDish(DishRequest request, ServerCallContext context) {
        return Task.FromResult(new DishReply() { Message = $"Answer {request.Code} was send"});
    }
}