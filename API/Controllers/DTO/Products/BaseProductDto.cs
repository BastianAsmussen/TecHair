namespace API.Controllers.DTO.Products;

public class BaseProductDto
{
    public int ProductId { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public IEnumerable<BasePriceDto> PriceHistory { get; set; }
}

public class BasePriceDto
{
    public int PriceId { get; set; }

    public decimal Value { get; set; }
    public DateTime Date { get; set; }
}
