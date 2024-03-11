using API.Controllers.DTO;
using API.Utility.Database.DAL;
using API.Utility.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new();

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromHeader(Name = "authorization")] string authorization)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var products = await _unitOfWork.ProductRepository.Get(includeProperties: "PriceHistory");

            return Ok(products);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(
        [FromHeader(Name = "authorization")] string authorization,
        [Bind("Name,Description,Image,Stock,Price")]
        NewProduct product)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            // Make sure that the image is a valid URL.
            if (!Uri.IsWellFormedUriString(product.Image, UriKind.Absolute))
                return BadRequest("Invalid image!");

            // If the stock is less than 0, return a bad request.
            if (product.Stock < 0) return BadRequest("Invalid stock!");

            // If the price is less than 0, return a bad request.
            if (product.Price < 0) return BadRequest("Invalid price!");

            var foundProduct = await _unitOfWork.ProductRepository.Get(p => p.Name.ToLower() == product.Name.ToLower());
            if (foundProduct != null && foundProduct.Any())
                return BadRequest("Product already exists!");

            var actualProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Stock = product.Stock,
                PriceHistory = new List<Price>
                {
                    new()
                    {
                        Value = product.Price,
                        Date = DateTime.UtcNow,
                    }
                }
            };

            _unitOfWork.ProductRepository.Insert(actualProduct);
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetProduct), new { id = actualProduct.ProductId }, actualProduct);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var products = await _unitOfWork.ProductRepository.Get(p => p.ProductId == id, includeProperties: "PriceHistory");
            if (products == null) return NotFound();

            return Ok(products.First());
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id))
                return NotFound();

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> PutProduct(
        [FromHeader(Name = "authorization")] string authorization,
        int id,
        [Bind("ProductId,Name,Description,Image,Stock,PriceHistory")]
        Product product)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        if (id != product.ProductId)
            return BadRequest();

        try
        {
            // Make sure that the image is a valid URL.
            if (!Uri.IsWellFormedUriString(product.Image, UriKind.Absolute))
                return BadRequest("Invalid image!");

            // If the stock is less than 0, return a bad request.
            if (product.Stock < 0) return BadRequest("Invalid stock!");
            // If all prices sum to less than 0, return a bad request.
            if (product.PriceHistory.Sum(p => p.Value) < 0)
                return BadRequest("Invalid price!");

            var foundProduct = await _unitOfWork.ProductRepository.Get(p => p.Name.ToLower() == product.Name.ToLower());

            // If the product already exists and the ID is not the same, return a bad request.
            if (foundProduct != null && foundProduct.Any(p => p.ProductId != product.ProductId))
                return BadRequest("Product already exists!");

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id)) return NotFound();

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if (product == null) return NotFound();

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id)) return NotFound();

            throw;
        }
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _unitOfWork.ProductRepository.Get(p => p.ProductId == id) != null;
    }
}