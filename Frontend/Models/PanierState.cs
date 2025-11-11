namespace Frontend.Models;

public class PanierState
{
    public PanierState(List<PanierItem> items, decimal total, int totalQuantity)
    {
        Items = items;
        Total = total;
        TotalQuantity = totalQuantity;
    }

    public List<PanierItem> Items { get; }
    public decimal Total { get; }
    public int TotalQuantity { get; }
}