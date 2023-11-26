using Microsoft.AspNetCore.SignalR;

namespace Restaurant.WebSite.Hubs;

public class RestaurantHub : Hub
{
    public async Task NotifyWebUsers(string user, string message) {
        await Clients.All.SendAsync("DisplayNotification", user, message);
    }
}