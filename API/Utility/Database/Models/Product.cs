namespace API.Utility.Database.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public IEnumerable<Price> PriceHistory { get; set; }
}

public class Price
{
    public int PriceId { get; set; }

    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}