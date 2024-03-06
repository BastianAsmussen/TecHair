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
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromHeader(Name = "authorization")] string authorization) {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try {
            var products = await _unitOfWork.ProductRepository.Get();

            return Ok(products);
        } catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(
        [FromHeader(Name = "authorization")] string authorization,
        [Bind("Name,Description,PriceHistory")]
        Product product) {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            if (product.PriceHistory.First().Value < 0) return BadRequest("Invalid price!");

            var foundProduct = await _unitOfWork.ProductRepository.Get(filter: p => p.Name == product.Name);
            if (foundProduct != null && foundProduct.Any())
                return BadRequest("Product already exists!");

            product.Description ??= "";

            _unitOfWork.ProductRepository.Insert(product);
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        } catch (DbUpdateConcurrencyException)
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
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        } catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id))
            {
                return NotFound();
            }

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> PutProduct(
        [FromHeader(Name = "authorization")] string authorization,
        int id,
        [Bind("ProductId,Name,Description,PriceHistory")]
        Product product)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        if (id != product.ProductId)
        {
            return BadRequest();
        }

        try
        {
            if (product.PriceHistory.First().Value < 0) return BadRequest("Invalid price!");
            if (await _unitOfWork.ProductRepository.Get(filter: p => p.Name == product.Name) != null)
                return BadRequest("Product already exists!");

            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.Save();

            return NoContent();
        } catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id))
            {
                return NotFound();
            }

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

        try {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Save();

            return NoContent();
        } catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExists(id))
            {
                return NotFound();
            }

            throw;
        }
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _unitOfWork.ProductRepository.Get(filter: p => p.ProductId == id) != null;
    }
}
