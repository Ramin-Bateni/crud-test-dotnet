using System.Collections.Generic;
using System.Threading.Tasks;
using Mc2.CrudTest.ApplicationServices.Models;
using Mc2.CrudTest.Domain.Customers.Commands;
using Mc2.CrudTest.Domain.Customers.Queries;
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
            if (!Validator.PhoneIsValid(customer.PhoneNumber))
            {
                return BadRequest("PhoneNumber is invalid!");
            }

            if (!Validator.EmailIsValid(customer.Email))
            {
                return BadRequest("Email is invalid!");
            }

            if (!Validator.BankAccountIsValid(customer.BankAccountNumber))
            {
                return BadRequest("Bank account is invalid!");
            }

            //------ Check same user is exist or not
            int customerId = await _mediator.Send(new GetCustomerIdByInfoQuery()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth
            });
            // If there is another person with same info
            if (customerId != 0 && customerId != customer.Id)
            {
                return BadRequest("Same customer is exist!");
            }
            //--------------------------------

            //------ Check is email exist or not
            customerId = await _mediator.Send(new GetCustomerIdByEmailQuery()
            {
                Email = customer.Email
            });
            // If there is another person with same email
            if (customerId != 0 && customerId != customer.Id)
            {
                return BadRequest("Same email is exist!");
            }
            //--------------------------------

            customer = await _mediator.Send(new CreateCustomerCommand()
            {
                FirstName = customer.FirstName,
                Lastname = customer.LastName,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber
            });
            return Ok(customer);
        }

        [HttpPut]
        [Route(nameof(Update))]
        public async Task<ActionResult> Update(Customer customer)
        {
            if (!Validator.PhoneIsValid(customer.PhoneNumber))
            {
                return BadRequest("PhoneNumber is invalid!");
            }

            if (!Validator.EmailIsValid(customer.Email))
            {
                return BadRequest("Email is invalid!");
            }

            if (!Validator.BankAccountIsValid(customer.BankAccountNumber))
            {
                return BadRequest("Bank account is invalid!");
            }

            //------ Check same user is exist or not
            int customerId = await _mediator.Send(new GetCustomerIdByInfoQuery()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth
            });
            // If there is another person with same info
            if (customerId != 0 && customerId != customer.Id)
            {
                return BadRequest("Same customer is exist!");
            }
            //--------------------------------

            //------ Check is email exist or not
            customerId = await _mediator.Send(new GetCustomerIdByEmailQuery()
            {
                Email = customer.Email
            });
            // If there is another person with same email
            if (customerId != 0 && customerId != customer.Id)
            {
                return BadRequest("Same email is exist!");
            }
            //--------------------------------

            int id = await _mediator.Send(new UpdateCustomerCommand()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber
            });
            return Ok(id);
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