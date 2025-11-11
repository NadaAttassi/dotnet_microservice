using StackExchange.Redis;
using System.Text.Json;
using PanierService.Models;
using PanierService.Requests;

namespace PanierService.Services;

public class RedisPanierService : IPanierService
{
    private readonly IDatabase _database;

    public RedisPanierService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<Panier> GetPanierAsync(string userId)
    {
        var panierJson = await _database.StringGetAsync($"panier:{userId}");
        
        if (!panierJson.HasValue)
            return new Panier();
        
        return JsonSerializer.Deserialize<Panier>(panierJson!) ?? new Panier();
    }

    public async Task<Panier> AddToPanierAsync(string userId, AddToPanierRequest request)
    {
        var panier = await GetPanierAsync(userId);
        
        var existingItem = panier.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += request.Quantity;
        }
        else
        {
            panier.Items.Add(new PanierItem
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Price = request.Price,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl
            });
        }
        
        await _database.StringSetAsync($"panier:{userId}", JsonSerializer.Serialize(panier));
        return panier;
    }

    public async Task<Panier> RemoveFromPanierAsync(string userId, int productId)
    {
        var panier = await GetPanierAsync(userId);
        panier.Items.RemoveAll(i => i.ProductId == productId);
        
        await _database.StringSetAsync($"panier:{userId}", JsonSerializer.Serialize(panier));
        return panier;
    }

    public async Task<Panier> UpdateQuantityAsync(string userId, int productId, int quantity)
    {
        var panier = await GetPanierAsync(userId);
        var item = panier.Items.FirstOrDefault(i => i.ProductId == productId);
        
        if (item != null)
        {
            item.Quantity = quantity;
            await _database.StringSetAsync($"panier:{userId}", JsonSerializer.Serialize(panier));
        }
        
        return panier;
    }

    public async Task ClearPanierAsync(string userId)
    {
        await _database.KeyDeleteAsync($"panier:{userId}");
    }
}