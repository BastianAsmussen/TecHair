using API.Controllers.DTO;
using API.Utility.Database.Models;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts() {
        return await context.Products
          .Select(p => new ProductDto(p))
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> PostProduct(ProductDto product) {
        context.Products.Add(Product.FromDto(product));
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return new ProductDto(product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductDto>> PutProduct(int id, ProductDto product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }

        var foundProduct = await context.Products.FindAsync(id);
        if (foundProduct == null)
        {
            return NotFound();
        }

        foundProduct = Product.FromDto(product);
        context.Products
            .Update(foundProduct);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!ProductExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        
        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(p => p.ProductId == id);
    }
}
