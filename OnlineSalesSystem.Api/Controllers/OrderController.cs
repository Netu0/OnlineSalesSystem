using Microsoft.AspNetCore.Mvc;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Services;
using OnlineSalesSystem.Api.DTOs;
using AutoMapper;

namespace OnlineSalesSystem.Api.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly CustomerService _customerService;
    private readonly IMapper _mapper;

    public OrdersController(OrderService orderService, CustomerService customerService, IMapper mapper)
    {
        _orderService = orderService;
        _customerService = customerService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponseDTO>> Create([FromBody] OrderCreateDTO orderDTO)
    {
        var order = _mapper.Map<Order>(orderDTO);
        var createdOrder = await _orderService.CreateAsync(order);
        var result = _mapper.Map<OrderResponseDTO>(createdOrder);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDTO>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();
        return _mapper.Map<OrderResponseDTO>(order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] OrderUpdateDTO orderDTO)
    {
        var existingOrder = await _orderService.GetByIdAsync(id);
        if (existingOrder == null) return NotFound();

        _mapper.Map(orderDTO, existingOrder);
        await _orderService.UpdateAsync(existingOrder);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OrderResponseDTO>>(orders));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null) return NotFound();

        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}