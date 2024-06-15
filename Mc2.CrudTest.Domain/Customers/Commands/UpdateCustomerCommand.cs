using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.Models;
using Mc2.CrudTest.Domain2.Services;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Commands
{
    [ExcludeFromCodeCoverage]
    public class UpdateCustomerCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }

        [ExcludeFromCodeCoverage]
        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, int>
        {
            private readonly ICustomerService _customerService;

            public UpdateCustomerCommandHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<int> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
            {
                Customer customer = await _customerService.GetCustomerByIdAsync(command.Id);
                if (customer == null)
                    return default;

                customer.FirstName = command.FirstName;
                customer.LastName = command.LastName;
                customer.Email = command.Email;
                customer.PhoneNumber = command.PhoneNumber;
                customer.BankAccountNumber = command.BankAccountNumber;

                return await _customerService.UpdateCustomerAsync(customer);
            }
        }
    }
}
