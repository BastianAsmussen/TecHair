using API.Utility.Database.Models;

namespace API.Controllers.DTO;

public class ProductDto(Product product)
{
    public int ProductId { get; } = product.ProductId;

    public string Name { get; } = product.Name;
    public string? Description { get; } = product.Description;

    public IEnumerable<PriceDto> PriceHistory { get; } = product.PriceHistory.Select(price => new PriceDto(price)).ToList();
}

public class PriceDto(Price price)
{
    public int PriceId { get; } = price.PriceId;

    public decimal Value { get; } = price.Value;
    public DateTime Date { get; } = price.Date;
}
