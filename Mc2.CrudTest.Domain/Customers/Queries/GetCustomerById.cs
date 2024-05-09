using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.ApplicationServices.Models;
using Mc2.CrudTest.ApplicationServices.Services;
using MediatR;

namespace Mc2.CrudTest.Domain.Customers.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public int Id { get; set; }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
        {
            private readonly ICustomerService _customerService;

            public GetCustomerByIdQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<Customer> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                return await _customerService.GetCustomerByIdAsync(query.Id);
            }
        }
    }
}
