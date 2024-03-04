using API.Controllers.DTO.Products;
using API.Utility.Database.Models;
using Database;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(DataContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BaseProductDto>>> GetProducts() {
        return await context.Products
          .Select(p => p.Adapt<BaseProductDto>())
          .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<BaseProductDto>> PostProduct(CreateProductDto productDto) {
        if (productDto.Price < 0) return BadRequest("Invalid price!");
        // If the product already exists, return a bad request.
        if (context.Products.Any(p => p.Name == productDto.Name)) return BadRequest("Product already exists!");
        
        productDto.Description ??= "";
        
        var product = productDto.Adapt<Product>();
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseProductDto>> GetProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return product.Adapt<BaseProductDto>();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseProductDto>> PutProduct(int id, UpdateProductDto product)
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

        if (product.Price < 0) return BadRequest("Invalid price!");
        if (context.Products.Any(p => p.Name == product.Name && p.ProductId != id)) return BadRequest("Product already exists!");

        product.Description ??= "";

        // Add the price to the price history.
        foundProduct.PriceHistory.Append(new Price
        {
            Value = product.Price,
            Date = DateTime.Now
        });

        foundProduct = product.Adapt(foundProduct);
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
