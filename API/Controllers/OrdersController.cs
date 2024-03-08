using API.Controllers.DTO;
using API.Utility.Database.DAL;
using API.Utility.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork = new();

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders(
        [FromHeader(Name = "authorization")] string authorization)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var orders = await _unitOfWork.OrderRepository.Get();

            return Ok(orders);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(
        [FromHeader(Name = "authorization")] string authorization,
        [Bind("AppointmentId,ProductIds")]
        NewOrder order)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            // First, make sure that the appointment exists.
            var appointment = await _unitOfWork.AppointmentRepository.GetById(order.AppointmentId);
            if (appointment == null) return BadRequest("Invalid appointment!");

            // Next, make sure that all products exist.
            var products = await _unitOfWork.ProductRepository.Get(p => order.ProductIds.Contains(p.ProductId));
            if (products == null || products.Count() != order.ProductIds.Count())
                return BadRequest("Invalid products!");

            var actualOrder = new Order { Date = DateTime.UtcNow, Appointment = appointment, Products = products.ToList() };

            _unitOfWork.OrderRepository.Insert(actualOrder);
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetOrder), new { id = actualOrder.OrderId }, actualOrder);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest();
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetOrder(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var orders = await _unitOfWork.OrderRepository.Get(o => o.OrderId == id, includeProperties: "Products");
            if (orders == null) return NotFound();

            return Ok(orders.First());
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrderExists(id))
                return NotFound();

            throw;
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> PutOrder(
        [FromHeader(Name = "authorization")] string authorization,
        int id,
        [Bind("OrderId,Date,Appointment,Products")]
        Order order)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        if (id != order.OrderId) return BadRequest();

        try
        {
            // First, make sure that the appointment exists.
            var appointment = await _unitOfWork.AppointmentRepository.GetById(order.Appointment.AppointmentId);
            if (appointment == null) return BadRequest("Invalid appointment!");

            // Next, make sure that all products exist.
            var products = await _unitOfWork.ProductRepository.Get(p => order.Products.Select(foundProduct => foundProduct.ProductId).Contains(p.ProductId));
            if (products == null || products.Count() != order.Products.Count())
                return BadRequest("Invalid products!");

            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrderExists(id)) return NotFound();

            throw;
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(
        [FromHeader(Name = "authorization")] string authorization,
        int id)
    {
        var auth = await Authorization.Validate(_unitOfWork, authorization, Role.Admin);
        if (auth == null) return Unauthorized();

        try
        {
            var order = await _unitOfWork.OrderRepository.GetById(id);
            if (order == null) return NotFound();

            _unitOfWork.OrderRepository.Delete(order);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await OrderExists(id)) return NotFound();

            throw;
        }
    }

    private async Task<bool> OrderExists(int id)
    {
        return await _unitOfWork.OrderRepository.Get(e => e.OrderId == id) != null;
    }
}