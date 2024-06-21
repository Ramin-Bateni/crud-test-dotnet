using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mc2.CrudTest.Application2.Customers.Commands;
using Mc2.CrudTest.Application2.Customers.Queries;
using Mc2.CrudTest.Domain2.Enums;
using Mc2.CrudTest.Domain2.Models;
using Mc2.CrudTest.Shared.Utilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Mc2.CrudTest.Presentation.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public CustomerController(ILogger<CustomerController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IEnumerable<Customer>> GetAll()
        {
            IEnumerable<Customer> result = await _mediator.Send(new GetAllCustomersQuery());
            return result;
        }

        [HttpGet]
        [Route(nameof(GetById)+"/{id}")]
        public async Task<Customer> GetById(int id)
        {
            return await _mediator.Send(new GetCustomerByIdQuery() { Id = id });
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<ActionResult> Create(Customer customer)
        {
            CreateOrUpdateResult<Customer> result = 
                await _mediator.Send(new CreateCustomerCommand()
            {
                FirstName = customer.FirstName,
                Lastname = customer.LastName,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber
            });

            if (result.HasError)
            {
                return BadRequest($"Validation Errors: {string.Join(", ", result.Errors)}");
            }

            return Ok(customer);
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(Customer customer)
        {
            CreateOrUpdateResult<Customer> result =
                await _mediator.Send(new UpdateCustomerCommand()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth,
                    PhoneNumber = customer.PhoneNumber,
                    BankAccountNumber = customer.BankAccountNumber
                });

            if (result.HasError)
            {
                return BadRequest($"Validation Errors: {string.Join(", ", result.Errors)}");
            }

            return Ok(customer);
        }

        [HttpDelete]
        [Route(nameof(Delete)+"/{id}")]
        public async Task<int> Delete(int id)
        {
            int resultId = await _mediator.Send(new DeleteCustomerCommand()
            {
                Id = id
            });
            return resultId;
        }
    }
}