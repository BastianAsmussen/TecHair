namespace API.Controllers.DTO.Products;

public class UpdateProductDto : BaseProductDto
{
    public int ProductId { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    public decimal Price { get; set; }
}