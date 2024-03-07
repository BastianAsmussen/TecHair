namespace API.Controllers.DTO;

public class NewProduct
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    public int Stock { get; set; }
    public decimal Price { get; set; }
}
