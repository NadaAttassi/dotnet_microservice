using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.Pages;

public class ProductsModel : PageModel
{
    private readonly CatalogService _catalogService;
    private readonly PanierService _panierService;

    public ProductsModel(CatalogService catalogService, PanierService panierService)
    {
        _catalogService = catalogService;
        _panierService = panierService;
    }

    public List<Product> Products { get; set; } = new();
    public string? UserEmail { get; set; }
    public int CartItemCount { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        UserEmail = "Guest User";
        Products = await _catalogService.GetProductsAsync();
        
        var panierState = await _panierService.GetPanierAsync();
        CartItemCount = panierState.TotalQuantity;
        
        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId)
    {
        var product = await _catalogService.GetProductByIdAsync(productId);
        if (product != null)
        {
            await _panierService.AddToPanierAsync(product);
        }
        return RedirectToPage();
    }
}
