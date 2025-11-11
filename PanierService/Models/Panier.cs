namespace PanierService.Models;

public class Panier
{
    public List<PanierItem> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Price * i.Quantity);
    public int TotalQuantity => Items.Count; // Nombre de produits différents
}