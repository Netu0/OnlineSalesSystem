using Microsoft.AspNetCore.Mvc;
using OnlineSalesSystem.Core.Entities;
using OnlineSalesSystem.Core.Interfaces;

namespace OnlineSalesSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(customers);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // Remove os orders recebidos para evitar criação em cascata não intencional
            customer.Orders = new List<Order>();

            await _customerRepository.AddAsync(customer);
            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // PUT: api/customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                await _customerRepository.UpdateAsync(customer);
            }
            catch (Exception)
            {
                if (await _customerRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerRepository.DeleteAsync(customer);

            return NoContent();
        }

        // GET: api/customers/5/orders
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetCustomerOrders(int id)
        {
            var orders = await _customerRepository.GetOrdersByCustomerIdAsync(id);
            return Ok(orders);
        }
    }
}