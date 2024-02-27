using Database;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts() {
        return await context.Products
          .Select(p => p)
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product) {
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Product>> GetProduct(long id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<Product>> PutProduct(long id, Product product)
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

        foundProduct = product;

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

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteProduct(long id)
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

    private bool ProductExists(long id)
    {
        return context.Products.Any(p => p.ProductId == id);
    }
}

