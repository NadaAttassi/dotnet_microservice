using StackExchange.Redis;
using PanierService.Services;
using PanierService.Requests;

var builder = WebApplication.CreateBuilder(args);

var redisConnection = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
var options = ConfigurationOptions.Parse(redisConnection);
options.AbortOnConnectFail = false;
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));
builder.Services.AddScoped<IPanierService, RedisPanierService>();

var app = builder.Build();

// Configuration du port pour Docker
if (app.Environment.IsDevelopment())
{
    app.Urls.Add("http://localhost:5003");
}
else
{
    app.Urls.Add("http://0.0.0.0:80");
}

app.MapGet("/api/panier/{userId}", async (string userId, IPanierService panierService) =>
{
    var panier = await panierService.GetPanierAsync(userId);
    return Results.Ok(panier);
});

app.MapPost("/api/panier/{userId}/items", async (string userId, AddToPanierRequest request, IPanierService panierService) =>
{
    var panier = await panierService.AddToPanierAsync(userId, request);
    return Results.Ok(panier);
});

app.MapDelete("/api/panier/{userId}/items/{productId}", async (string userId, int productId, IPanierService panierService) =>
{
    var panier = await panierService.RemoveFromPanierAsync(userId, productId);
    return Results.Ok(panier);
});

app.MapPut("/api/panier/{userId}/items/{productId}", async (string userId, int productId, UpdateQuantityRequest request, IPanierService panierService) =>
{
    if (request.Quantity <= 0)
    {
        var panier = await panierService.RemoveFromPanierAsync(userId, productId);
        return Results.Ok(panier);
    }
    
    // Logique pour update quantity (à implémenter dans le service)
    var updatedPanier = await panierService.UpdateQuantityAsync(userId, productId, request.Quantity);
    return Results.Ok(updatedPanier);
});

app.MapDelete("/api/panier/{userId}", async (string userId, IPanierService panierService) =>
{
    await panierService.ClearPanierAsync(userId);
    return Results.Ok();
});

app.Run();

public class UpdateQuantityRequest
{
    public int Quantity { get; set; }
}