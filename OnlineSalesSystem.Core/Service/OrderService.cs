using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Interfaces;

namespace OnlineSalesSystem.Core.Services;

public class OrderService(IOrderRepository orderRepository)
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _orderRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Order order)
    {
        await _orderRepository.AddAsync(order);
    }

    public async Task UpdateAsync(Order order)
    {
        await _orderRepository.UpdateAsync(order);
    }

    public async Task DeleteAsync(int id)
    {
        await _orderRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
    {
        return await _orderRepository.GetByCustomerIdAsync(customerId);
    }
}