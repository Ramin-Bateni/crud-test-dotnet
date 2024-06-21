using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Mc2.CrudTest.Application2.Services;
using Mc2.CrudTest.Domain2.Enums;
using Mc2.CrudTest.Domain2.Models;
using MediatR;

namespace Mc2.CrudTest.Application2.Customers.Queries
{
    [ExcludeFromCodeCoverage]
    public class GetValidateErrorsForCreateUpdateCustomerQuery : IRequest<List<CustomerCrudErrors>>
    {
        public Customer Customer { get; set; }

        public class GetValidateErrorsForCreateUpdateCustomerQueryHandler 
            : IRequestHandler<GetValidateErrorsForCreateUpdateCustomerQuery, List<CustomerCrudErrors>>
        {
            private readonly ICustomerService _customerService;

            public GetValidateErrorsForCreateUpdateCustomerQueryHandler(ICustomerService customerService)
            {
                _customerService = customerService;
            }

            public async Task<List<CustomerCrudErrors>> Handle(GetValidateErrorsForCreateUpdateCustomerQuery query, CancellationToken cancellationToken)
            {
                return await _customerService.GetValidateErrorsForCreateNewCustomer(query.Customer);
            }
        }
    }
}
