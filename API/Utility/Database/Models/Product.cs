namespace Database.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public List<Price> PriceHistory { get; set; } = new();
}

public class Price
{
    public int PriceId { get; set; }

    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}
