using System.Text;
using System.Text.Json;
using Frontend.Models;

namespace Frontend.Services;

public class PanierService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PanierService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        var baseUrl = _configuration["Services:PanierService"];
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    // Récupère le panier pour un utilisateur donné 
    public async Task<PanierState> GetPanierAsync(string userId = "guest")
    {
        var response = await _httpClient.GetAsync($"/api/panier/{userId}");
        if (response.IsSuccessStatusCode)
        {
            // on met la réponse dans un objet de type json
            var json = await response.Content.ReadAsStringAsync();
            // on désérialise le json dans un objet PanierResponse
            var panier = JsonSerializer.Deserialize<PanierResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            // on transfert les données de l'objet PanierResponse dans un objet de type PanierState
            return new PanierState(panier?.Items ?? new(), panier?.Total ?? 0, panier?.TotalQuantity ?? 0);
        }
        return new PanierState(new(), 0, 0);
    }


    public async Task<PanierState> AddToPanierAsync(Product product, int quantity = 1, string userId = "guest")
    {
        var request = new AddToPanierRequest
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = quantity,
            ImageUrl = product.ImageUrl
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"/api/panier/{userId}/items", content);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var panier = JsonSerializer.Deserialize<PanierResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new PanierState(panier?.Items ?? new(), panier?.Total ?? 0, panier?.TotalQuantity ?? 0);
        }
        return new PanierState(new(), 0, 0);
    }

    public async Task<PanierState> RemoveFromPanierAsync(int productId, string userId = "guest")
    {
        var response = await _httpClient.DeleteAsync($"/api/panier/{userId}/items/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var panier = JsonSerializer.Deserialize<PanierResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new PanierState(panier?.Items ?? new(), panier?.Total ?? 0, panier?.TotalQuantity ?? 0);
        }
        return new PanierState(new(), 0, 0);
    }

    public async Task<PanierState> UpdateQuantityAsync(int productId, int quantity, string userId = "guest")
    {
        if (quantity <= 0)
        {
            return await RemoveFromPanierAsync(productId, userId);
        }
        
        var request = new { Quantity = quantity };
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PutAsync($"/api/panier/{userId}/items/{productId}", content);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var panier = JsonSerializer.Deserialize<PanierResponse>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return new PanierState(panier?.Items ?? new(), panier?.Total ?? 0, panier?.TotalQuantity ?? 0);
        }
        return new PanierState(new(), 0, 0);
    }

    public async Task ClearPanierAsync(string userId = "guest")
    {
        await _httpClient.DeleteAsync($"/api/panier/{userId}");
    }
}

