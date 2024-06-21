using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Application2.Services;
using Mc2.CrudTest.Domain2.Enums;
using Mc2.CrudTest.Domain2.Models;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Commands
{
    [ExcludeFromCodeCoverage]
    public class CreateCustomerCommand : IRequest<CreateOrUpdateResult<Customer>>
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        [ExcludeFromCodeCoverage]
        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CreateOrUpdateResult<Customer>>
        {
            private readonly ICustomerService _customerService;

            public CreateCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<CreateOrUpdateResult<Customer>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
            {
                Customer customer = new Customer()
                {
                    FirstName = command.FirstName,
                    LastName = command.Lastname,
                    Email = command.Email,
                    DateOfBirth = command.DateOfBirth,
                    PhoneNumber = command.PhoneNumber,
                    BankAccountNumber = command.BankAccountNumber
                };

                return await _customerService.CreateCustomerAsync(customer);
            }
        }
    }
}
