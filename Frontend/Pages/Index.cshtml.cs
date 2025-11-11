using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        // Redirect directly to products
        return RedirectToPage("/Products");
    }
}
