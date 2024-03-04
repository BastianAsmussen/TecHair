using API.Controllers.DTO;

namespace API.Utility.Database.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public IEnumerable<Price> PriceHistory { get; set; }

    public static Product FromDto(ProductDto dto)
    {
        return new Product
        {
            ProductId = dto.ProductId,

            Name = dto.Name,
            Description = dto.Description,

            PriceHistory = dto.PriceHistory.Select(Price.FromDto).ToList()
        };
    }
}

public class Price
{
    public int PriceId { get; set; }

    public decimal Value { get; set; }
    public DateTime Date { get; set; }

    public static Price FromDto(PriceDto dto)
    {
        return new Price
        {
            PriceId = dto.PriceId,

            Value = dto.Value,
            Date = dto.Date
        };
    }
}
