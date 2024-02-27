using Lesson07.Data;
using Lesson07.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson07.Stores
{
    public class CustomersDataStore:IDisposable
    {
        private readonly InventoryDbContext _context;

        public CustomersDataStore()
        {
            _context = new InventoryDbContext();
        }

        public async Task<List<Customer>> GetCustomersAsync(int pageSize, int currentPage, string? searchString = null, int? customerId = null)
        {
            var query = _context.Customers
                .AsQueryable(); 

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x => (x.FirstName + x.LastName + x.Address).Contains(searchString));
            }

            if (customerId is not null and > 0)
            {
                query = query.Where(x => x.Id == customerId);
            }

            var customers = await query.Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return customers;
        }
        public async Task<List<Customer>> GetCustomersAsync(string? searchString = null, int? customerId = null)
        {
            var query = _context.Customers
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(x => (x.FirstName + x.LastName + x.Address).Contains(searchString));
            }

            if (customerId is not null and > 0)
            {
                query = query.Where(x => x.Id == customerId);
            }

            var customers = await query.ToListAsync();
            return customers;
        }

        public async Task<int> GetCustomersCountAsync() => await _context.Customers.CountAsync();

        public Customer CreateCustomer(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            var createdCustomer = _context.Customers.Add(customer);
            _context.SaveChanges();

            return createdCustomer.Entity;
        }

        public void UpdateCustomer(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            if (!EntityExists(customer.Id))
            {
                throw new InvalidOperationException($"Customer with id: {customer.Id} not exist");
            }

            _context.Customers.Attach(customer);
            _context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> DeleteCustomer(int id)
        {
            if (!EntityExists(id))
            {
                throw new InvalidOperationException($"Customer with id: {id} does not exist");
            }

            var customer = _context.Customers.First(x => x.Id == id);
            _context.Customers.Remove(customer);
            return await _context.SaveChangesAsync();
        }

        private bool EntityExists(int id) => _context.Customers.Any(x => x.Id == id);

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
