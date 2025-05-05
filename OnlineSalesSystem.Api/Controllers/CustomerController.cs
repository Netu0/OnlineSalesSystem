using Microsoft.AspNetCore.Mvc;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Services;
using OnlineSalesSystem.Api.DTOs;

namespace OnlineSalesSystem.Api.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController(CustomerService customerService) : ControllerBase
{
    private readonly CustomerService _customerService = customerService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        
        if (customer == null)
            return NotFound();
            
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CustomerCreateDTO customerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var customer = new Customer
        {
            Name = customerDTO.Name,
            Email = customerDTO.Email,
            Phone = customerDTO.Phone
        };

        await _customerService.AddAsync(customer);
        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Update(int id, [FromBody] CustomerUpdateDTO customerDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingCustomer = await _customerService.GetByIdAsync(id);
        if (existingCustomer == null)
            return NotFound();

        existingCustomer.Name = customerDTO.Name;
        existingCustomer.Email = customerDTO.Email;
        existingCustomer.Phone = customerDTO.Phone;

        await _customerService.UpdateAsync(existingCustomer);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound();

        await _customerService.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id:int}/orders")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int id)
    {
        var orders = await _customerService.GetOrdersByCustomerIdAsync(id);
        return Ok(orders);
    }
}