using OnlineSalesSystem.Core.Entities;

namespace OnlineSalesSystem.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(int customerId);
    }
}