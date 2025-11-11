using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Services;
using Frontend.Models;

namespace Frontend.Pages;

public class CartModel : PageModel
{
    private readonly PanierService _panierService;

    public CartModel(PanierService panierService)
    {
        _panierService = panierService;
    }

    public List<PanierItem> CartItems { get; set; } = new();
    public decimal Total { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var panierState = await _panierService.GetPanierAsync();
        CartItems = panierState.Items;
        Total = panierState.Total;
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync(int productId, int quantity)
    {
        await _panierService.UpdateQuantityAsync(productId, quantity);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int productId)
    {
        await _panierService.RemoveFromPanierAsync(productId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostClearAsync()
    {
        await _panierService.ClearPanierAsync();
        return RedirectToPage();
    }
}