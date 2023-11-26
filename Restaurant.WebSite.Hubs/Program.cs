using Restaurant.WebSite.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddPolicy(name: "Hub",
    policy =>
    {
        policy.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    }));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("Hub");

app.MapControllers();

app.MapHub<RestaurantHub>("/hub");

app.Run();