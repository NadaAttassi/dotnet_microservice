using Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages
builder.Services.AddRazorPages();

// Add HttpContextAccessor for cookie access
builder.Services.AddHttpContextAccessor();

// Register HTTP clients for microservices
builder.Services.AddHttpClient<CatalogService>();
builder.Services.AddHttpClient<PanierService>();



var app = builder.Build();

// Configuration du port pour Docker
if (app.Environment.IsDevelopment())
{
    app.Urls.Add("http://localhost:5000");
}
else
{
    app.Urls.Add("http://0.0.0.0:80");
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// Middleware pipeline
// Configure authentication and authorization
app.UseStaticFiles();
// Enable routing and authorization
app.UseRouting();
app.UseAuthorization();
// Map Razor Pages
app.MapRazorPages();

app.Run();
