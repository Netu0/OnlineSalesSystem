using Microsoft.AspNetCore.Mvc;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Services;
using OnlineSalesSystem.Api.DTOs;

namespace OnlineSalesSystem.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(OrderService orderService, CustomerService customerService) : ControllerBase
{
    private readonly OrderService _orderService = orderService;
    private readonly CustomerService _customerService = customerService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Order>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        
        if (order == null)
            return NotFound();
            
        return Ok(order);
    }

    [HttpGet("by-customer/{customerId:int}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetByCustomer(int customerId)
    {
        var orders = await _orderService.GetByCustomerIdAsync(customerId);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] OrderCreateDTO orderDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = await _customerService.GetByIdAsync(orderDTO.CustomerId);
        if (customer == null)
            return BadRequest("Customer not found");

        var order = new Order
        {
            CustomerId = orderDTO.CustomerId,
            Customer = customer,
            OrderDate = orderDTO.OrderDate,
            Total = orderDTO.Total
        };

        await _orderService.AddAsync(order);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] OrderUpdateDTO orderDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingOrder = await _orderService.GetByIdAsync(id);
        if (existingOrder == null)
            return NotFound();

        existingOrder.OrderDate = orderDTO.OrderDate;
        existingOrder.Total = orderDTO.Total;

        await _orderService.UpdateAsync(existingOrder);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound();

        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}