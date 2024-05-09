using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.ApplicationServices.Models;
using Mc2.CrudTest.ApplicationServices.Services;
using MediatR;

namespace Mc2.CrudTest.Domain.Customers.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>>
    {
        public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
        {
            private readonly ICustomerService _customerService;

            public GetAllCustomersQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery query, CancellationToken cancellationToken)
            {
                return await _customerService.GetCustomersListAsync();
            }
        }
    }
}
