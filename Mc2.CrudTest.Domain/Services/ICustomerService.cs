using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.Enums;
using Mc2.CrudTest.Domain2.Models;

namespace Mc2.CrudTest.Application2.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetCustomersListAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<CreateOrUpdateResult<Customer>> CreateCustomerAsync(Customer customer);
        Task<CreateOrUpdateResult<Customer>> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int customerId);
        Task<int> GetCustomerIdOfCustomerAsync(string firstName, string lastName, DateTime dateOfBirth);
        Task<int> GetCustomerIdByEmailAsync(string email);
        Task<List<CustomerCrudErrors>> GetValidateErrorsForCreateNewCustomer(Customer customer);
        Task<IEnumerable<Customer>> GetCustomersByInfoAsync(Customer customer);
    }
}
