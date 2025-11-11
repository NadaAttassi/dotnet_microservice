using PanierService.Models;
using PanierService.Requests;

namespace PanierService.Services;

public interface IPanierService
{
    Task<Panier> GetPanierAsync(string userId);
    Task<Panier> AddToPanierAsync(string userId, AddToPanierRequest request);
    Task<Panier> RemoveFromPanierAsync(string userId, int productId);
    Task<Panier> UpdateQuantityAsync(string userId, int productId, int quantity);
    Task ClearPanierAsync(string userId);
}