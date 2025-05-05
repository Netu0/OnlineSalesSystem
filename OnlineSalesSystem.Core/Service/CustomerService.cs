using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Interfaces;

namespace OnlineSalesSystem.Core.Services;

public class CustomerService(ICustomerRepository customerRepository)
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateAsync(Customer customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteAsync(int id)
    {
        await _customerRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        return await _customerRepository.GetOrdersByCustomerIdAsync(customerId);
    }
}