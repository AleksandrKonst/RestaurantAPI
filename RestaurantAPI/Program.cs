using GraphQL.Server;
using GraphQL.Types;
using RestaurantAPI.Data;
using RestaurantAPI.GraphQL.Schemas;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRestaurantStorage, RestaurantStorage>();

builder.Services.AddScoped<ISchema, RestaurantSchema>();
builder.Services.AddGraphQL(options => { options.EnableMetrics = true; }).AddSystemTextJson();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseGraphQLAltair();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseGraphQL<ISchema>();

app.MapControllers();

app.Run();