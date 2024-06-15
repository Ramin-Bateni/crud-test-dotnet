using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.Models;

namespace Mc2.CrudTest.Domain2.IRepositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetCustomersListAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int customerId);
        Task<int> GetCustomerIdOfCustomerAsync(string firstName, string lastName, DateTime dateOfBirth);
        Task<int> GetCustomerIdByEmailAsync(string email);
    }
}
