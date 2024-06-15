using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.IRepositories;
using Mc2.CrudTest.Domain2.Models;

namespace Mc2.CrudTest.Domain2.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repo;

        public CustomerService(IUnitOfWork unitOfWork, ICustomerRepository repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }

        public async Task<IEnumerable<Customer>> GetCustomersListAsync()
        {
            return await _repo.GetCustomersListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _repo.GetCustomerByIdAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            await _repo.CreateCustomerAsync(customer);
            await _unitOfWork.SaveAsync();
            return customer;
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            return await _repo.UpdateCustomerAsync(customer);
        }

        public async Task<int> DeleteCustomerAsync(int customerId)
        {
            return await _repo.DeleteCustomerAsync(customerId);
        }

        public async Task<int> GetCustomerIdOfCustomerAsync(string firstName, string lastName, DateTime dateOfBirth)
        {
            return await _repo.GetCustomerIdOfCustomerAsync(firstName, lastName, dateOfBirth);
        }

        public async Task<int> GetCustomerIdByEmailAsync(string email)
        {
            return await _repo.GetCustomerIdByEmailAsync(email);
        }
    }
}
