namespace Frontend.Models;

public class PanierResponse
{
    public List<PanierItem> Items { get; set; } = new();
    public decimal Total { get; set; }
    public int TotalQuantity { get; set; }
}