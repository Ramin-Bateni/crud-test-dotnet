using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.ApplicationServices.Models;
using Mc2.CrudTest.ApplicationServices.Services;
using MediatR;

namespace Mc2.CrudTest.Domain.Customers.Commands
{
    [ExcludeFromCodeCoverage]
    public class CreateCustomerCommand : IRequest<ApplicationServices.Models.Customer>
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        [ExcludeFromCodeCoverage]
        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, ApplicationServices.Models.Customer>
        {
            private readonly ICustomerService _customerService;

            public CreateCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<ApplicationServices.Models.Customer> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
            {
                Customer customer = new ApplicationServices.Models.Customer()
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
