using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Application2.Services;
using Mc2.CrudTest.Domain2.Models;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCustomersByInfoQuery : IRequest<IEnumerable<Customer>>
    {
        public Customer Customer { get; set; }

        public class GetCustomersByInfoQueryHandler : IRequestHandler<GetCustomersByInfoQuery, IEnumerable<Customer>>
        {
            private readonly ICustomerService _customerService;

            public GetCustomersByInfoQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<IEnumerable<Customer>> Handle(GetCustomersByInfoQuery query, CancellationToken cancellationToken)
            {
                return await _customerService.GetCustomersByInfoAsync(query.Customer);
            }
        }
    }
}
