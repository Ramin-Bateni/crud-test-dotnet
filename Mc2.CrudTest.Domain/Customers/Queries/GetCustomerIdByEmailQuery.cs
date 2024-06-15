using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Domain2.Services;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCustomerIdByEmailQuery : IRequest<int>
    {
        public string Email { get; set; }

        public class GetCustomerIdByEmailQueryHandler : IRequestHandler<GetCustomerIdByEmailQuery, int>
        {
            private readonly ICustomerService _customerService;

            public GetCustomerIdByEmailQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<int> Handle(GetCustomerIdByEmailQuery query, CancellationToken cancellationToken)
            {
                return await _customerService.GetCustomerIdByEmailAsync(query.Email);
            }
        }
    }
}
