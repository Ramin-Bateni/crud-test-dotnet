using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.IRepositories;
using Mc2.CrudTest.Domain2.Models;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetCustomersListAsync()
        {
            var items = await _context.Customer.ToListAsync();
            return items;
        }

        public async Task<IEnumerable<Customer>> GetCustomersByInfoAsync(Customer customer)
        {
            var items = await _context.Customer
                .Where(x=>
                    string.Equals(x.FirstName, customer.FirstName, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.LastName, customer.LastName, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.Email, customer.Email, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.PhoneNumber, customer.PhoneNumber, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.BankAccountNumber, customer.BankAccountNumber, StringComparison.CurrentCultureIgnoreCase) &&
                    x.DateOfBirth == customer.DateOfBirth
                    )
                .ToListAsync();

            return items;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customer
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _context.Customer.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            _context.Customer.Update(customer);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCustomerAsync(int customerId)
        {
            Customer customer = await GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                return default;
            }

            _context.Customer.Remove(customer);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> GetCustomerIdOfCustomerAsync(string firstName, string lastName, DateTime dateOfBirth)
        {
            Customer customer = await _context.Customer.FirstOrDefaultAsync(x =>
                x.FirstName.ToLower() == firstName.ToLower() &&
                x.LastName.ToLower() == lastName.ToLower() &&
                x.DateOfBirth == dateOfBirth);
            return customer?.Id ?? 0;
        }

        public async Task<int> GetCustomerIdByEmailAsync(string email)
        {
            Customer customer = await _context.Customer
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            return customer?.Id ?? 0;
        }
    }
}