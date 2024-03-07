namespace API.Utility.Database.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public IEnumerable<Price> PriceHistory { get; set; }
}
