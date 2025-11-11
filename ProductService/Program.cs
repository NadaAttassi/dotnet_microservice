using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configuration du port pour Docker
if (app.Environment.IsDevelopment())
{
    app.Urls.Add("http://localhost:5002");
}
else
{
    app.Urls.Add("http://0.0.0.0:80");
}

// Initialiser la base de données
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await context.Database.EnsureCreatedAsync();
    
    // Ajouter les produits si la table est vide
    if (!await context.Products.AnyAsync())
    {
        var products = new[]
        {
            new ProductService.Models.Product { Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Stock = 10, ImageUrl = "https://images.unsplash.com/photo-1517336714731-489689fd1ca8?w=800" },
            new ProductService.Models.Product { Name = "Smartphone", Description = "Latest smartphone", Price = 699.99m, Stock = 15, ImageUrl = "https://images.unsplash.com/photo-1591337676887-a217a6970a8a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=580" },
            new ProductService.Models.Product { Name = "Headphones", Description = "Wireless headphones", Price = 199.99m, Stock = 25, ImageUrl = "https://plus.unsplash.com/premium_photo-1679513691474-73102089c117?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=813" },
            new ProductService.Models.Product { Name = "Tablet", Description = "10-inch tablet", Price = 399.99m, Stock = 8, ImageUrl = "https://images.unsplash.com/photo-1623126908029-58cb08a2b272?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=870" },
            new ProductService.Models.Product { Name = "Smartwatch", Description = "Fitness smartwatch", Price = 299.99m, Stock = 12, ImageUrl = "https://images.unsplash.com/photo-1579586337278-3befd40fd17a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=872" },
            new ProductService.Models.Product { Name = "Gaming Mouse", Description = "RGB gaming mouse", Price = 79.99m, Stock = 30, ImageUrl = "https://images.unsplash.com/photo-1613141412501-9012977f1969?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=870" },
            new ProductService.Models.Product { Name = "Keyboard", Description = "Mechanical keyboard", Price = 149.99m, Stock = 20, ImageUrl = "https://plus.unsplash.com/premium_photo-1664194583917-b0ba07c4ce2a?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=870" },
            new ProductService.Models.Product { Name = "Monitor", Description = "27-inch 4K monitor", Price = 449.99m, Stock = 5, ImageUrl = "https://images.unsplash.com/photo-1593640408182-31c70c8268f5?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=842" }
        };
        
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}

// Get all products endpoint
app.MapGet("/api/products", async (CatalogDbContext db) =>
{
    var products = await db.Products.ToListAsync();
    return Results.Ok(products);
});

// Get single product by ID endpoint
app.MapGet("/api/products/{id}", async (int id, CatalogDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

app.Run();