using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mc2.CrudTest.Application2.Customers.Commands;
using Mc2.CrudTest.Application2.Customers.Queries;
using Mc2.CrudTest.Domain2;
using Mc2.CrudTest.Domain2.Enums;
using Mc2.CrudTest.Domain2.IRepositories;
using Mc2.CrudTest.Domain2.Models;
using Mc2.CrudTest.Shared.Utilities;
using MediatR;

namespace Mc2.CrudTest.Application2.Services
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

        public async Task<CreateOrUpdateResult<Customer>> CreateCustomerAsync(Customer customer)
        {
            List<CustomerCrudErrors> errors = await GetValidateErrorsForCreateNewCustomer(customer);
            
            // If we have some errors
            if (errors.Any())
            {
                return CreateOrUpdateResult<Customer>.AddErrors(errors.Select(x=>(int)x));
            }

            await _repo.CreateCustomerAsync(customer);
            await _unitOfWork.SaveAsync();

            return new CreateOrUpdateResult<Customer>(customer);
        }

        public async Task<CreateOrUpdateResult<Customer>> UpdateCustomerAsync(Customer customer)
        {
            List<CustomerCrudErrors> errors = await GetValidateErrorsForCreateNewCustomer(customer);

            // If we have some errors
            if (errors.Any())
            {
                return CreateOrUpdateResult<Customer>.AddErrors(errors.Select(x => (int)x));
            }

            await _repo.UpdateCustomerAsync(customer);

            return new CreateOrUpdateResult<Customer>(customer);
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

        public async Task<IEnumerable<Customer>> GetCustomersByInfoAsync(Customer customer)
        {
            return await _repo.GetCustomersByInfoAsync(customer);
        }

        public async Task<List<CustomerCrudErrors>> GetValidateErrorsForCreateNewCustomer(Customer customer)
        {
            List<int> validationErrors = new();
            List<CustomerCrudErrors> toReturnErrorCodes = new();

            if (!Validator.PhoneIsValid(customer.PhoneNumber))
            {
                toReturnErrorCodes.Add(CustomerCrudErrors.InvalidPhoneNumber);
            }

            if (!Validator.EmailIsValid(customer.Email))
            {
                toReturnErrorCodes.Add(CustomerCrudErrors.InvalidEmail);
            }

            if (!Validator.BankAccountIsValid(customer.BankAccountNumber))
            {
                toReturnErrorCodes.Add(CustomerCrudErrors.InvalidBankAccountNumber);
            }

            //------ Check same user is exist or not
            int customerId = await GetCustomerIdOfCustomerAsync(
                customer.FirstName,
                customer.LastName,
                customer.DateOfBirth);

            // If there is another person with same info
            if (customerId != 0 && customerId != customer.Id)
            {
                toReturnErrorCodes.Add(CustomerCrudErrors.DuplicatedFirstNameLastnamePhoneNo);
            }
            //--------------------------------

            //------ Check is email exist or not
            customerId = await GetCustomerIdByEmailAsync(customer.Email);

            // If there is another person with same email
            if (customerId != 0 && customerId != customer.Id)
            {
                toReturnErrorCodes.Add(CustomerCrudErrors.DuplicatedEmail);
            }
            //--------------------------------

            return toReturnErrorCodes;
        }
    }
}
