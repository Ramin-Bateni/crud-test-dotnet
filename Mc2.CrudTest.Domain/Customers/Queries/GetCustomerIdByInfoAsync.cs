using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Application2.Services;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetCustomerIdByInfoQuery : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public class GetCustomerIdByInfoQueryHandler : IRequestHandler<GetCustomerIdByInfoQuery, int>
        {
            private readonly ICustomerService _customerService;

            public GetCustomerIdByInfoQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<int> Handle(GetCustomerIdByInfoQuery query, CancellationToken cancellationToken)
            {
                return await _customerService.GetCustomerIdOfCustomerAsync(
                    query.FirstName,
                    query.LastName,
                    query.DateOfBirth);
            }
        }
    }
}
