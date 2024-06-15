using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.Models;
using Mc2.CrudTest.Domain2.Services;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Queries
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
