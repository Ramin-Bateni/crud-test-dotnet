using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Mc2.CrudTest.Application2.Customers.Commands;
using Mc2.CrudTest.Application2.Customers.Queries;
using Mc2.CrudTest.Domain2.Models;
using Mc2.CrudTest.Presentation.Server.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.Presentation.Server.Controllers
{
    [ExcludeFromCodeCoverage]
    public class CustomerControllerTest
    {
        private readonly CustomerController _controller;
        private readonly IMediator _mediator;
        private readonly Customer _customer;
        private readonly int _secondCustomerId;

        public CustomerControllerTest()
        {
            // A valid customer
            const int firstCustomerId = 1;
            _secondCustomerId = 2;

            _customer = new Customer
            {
                Id = firstCustomerId,
                FirstName = "Ramin",
                LastName = "Bateni",
                PhoneNumber = "2024561111",
                Email = "a@a.com",
                BankAccountNumber = "0123456789",
                DateOfBirth = DateTime.Now,
            };
            ILogger<CustomerController> logger = Substitute.For<ILogger<CustomerController>>();
            _mediator = Substitute.For<IMediator>();

            _controller = new CustomerController(logger, _mediator);
        }

        [Fact]
        public async Task GetAll_WhenCall_ReturnsIEnumerableOfCustomer()
        {
            // Act
            IEnumerable<Customer> result = await _controller.GetAll();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<Customer>>();
        }

        [Fact]
        public async Task GetById_WhenCall_ReturnsCustomer()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetCustomerByIdQuery>()).Returns(_customer);

            // Act
            Customer result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<Customer>();
        }

        [Fact]
        public async Task CreateCustomer_WhenCustomerIsValid_ReturnsOkObjectResult()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetCustomerIdByInfoQuery>()).Returns(0);
            _mediator.Send(Arg.Any<GetCustomerIdByEmailQuery>()).Returns(0);
            _mediator.Send(new CreateCustomerCommand()).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Create(_customer);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateCustomer_WhenPhoneNumberIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            CreateCustomerCommand createCustomerCommand = new();
            _customer.PhoneNumber = "1"; // Invalid phone number

            _mediator.Send(createCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Create(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCustomer_WhenEmailIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            CreateCustomerCommand createCustomerCommand = new();
            _customer.Email = "myname"; // Invalid email

            _mediator.Send(createCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Create(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCustomer_WhenBankAccountIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            CreateCustomerCommand createCustomerCommand = new();
            _customer.BankAccountNumber = "1"; // Invalid bank account

            _mediator.Send(createCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Create(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCustomer_WhenSameCustomerIsExist_ReturnsBadRequestObjectResult()
        {
            // Arrange
            CreateCustomerCommand createCustomerCommand = new();

            _mediator.Send(Arg.Any<GetCustomerIdByInfoQuery>()).ReturnsForAnyArgs(_secondCustomerId);
            _mediator.Send(createCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Create(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCustomer_WhenSameEmailIsExist_ReturnsBadRequestObjectResult()
        {
            // Arrange
            CreateCustomerCommand createCustomerCommand = new();

            _mediator.Send<int>(Arg.Any<GetCustomerIdByInfoQuery>()).Returns(0);
            _mediator.Send(Arg.Any<GetCustomerIdByEmailQuery>()).Returns(_secondCustomerId);
            _mediator.Send(createCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Create(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenCustomerIsValid_ReturnsOkObjectResult()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetCustomerIdByInfoQuery>()).Returns(0);
            _mediator.Send(Arg.Any<GetCustomerIdByEmailQuery>()).Returns(0);
            _mediator.Send(Arg.Any<UpdateCustomerCommand>()).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Update(_customer);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenPhoneNumberIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            UpdateCustomerCommand updateCustomerCommand = new();
            _customer.PhoneNumber = "1"; // Invalid phone number

            _mediator.Send(updateCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Update(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenEmailIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            UpdateCustomerCommand updateCustomerCommand = new();
            _customer.Email = "myname"; // Invalid email

            _mediator.Send(updateCustomerCommand).ReturnsForAnyArgs(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Update(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenBankAccountIsInvalid_ReturnsBadRequestObjectResult()
        {
            // Arrange
            UpdateCustomerCommand updateCustomerCommand = new();
            _customer.Email = "myname"; // Invalid email

            _mediator.Send(updateCustomerCommand).ReturnsForAnyArgs(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Update(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenSameCustomerIsExist_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetCustomerIdByInfoQuery>()).Returns(_secondCustomerId);
            _mediator.Send(Arg.Any<GetCustomerIdByEmailQuery>()).Returns(0);
            _mediator.Send(Arg.Any<UpdateCustomerCommand>()).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Update(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task UpdateCustomer_WhenSameEmailIsExist_ReturnsBadRequestObjectResult()
        {
            // Arrange
            UpdateCustomerCommand updateCustomerCommand = new();
            _mediator.Send(Arg.Any<GetCustomerIdByInfoQuery>()).Returns(0);
            _mediator.Send(Arg.Any<GetCustomerIdByEmailQuery>()).Returns(_secondCustomerId);
            _mediator.Send(updateCustomerCommand).Returns(CreateOrUpdateResult<Customer>.SetModel(_customer));

            // Act
            ActionResult result = await _controller.Update(_customer);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteCustomer_ItemFound_Returns1()
        {
            // Arrange
            _mediator.Send(Arg.Any<DeleteCustomerCommand>()).Returns(1);

            // Act
            int result = await _controller.Delete(1);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public async Task DeleteCustomer_ItemNotFound_Returns0()
        {
            // Arrange
            DeleteCustomerCommand deleteCustomerCommand = new();

            _mediator.Send(deleteCustomerCommand).Returns(0);

            // Act
            int result = await _controller.Delete(100);

            // Assert
            result.Should().Be(0);
        }
    }
}