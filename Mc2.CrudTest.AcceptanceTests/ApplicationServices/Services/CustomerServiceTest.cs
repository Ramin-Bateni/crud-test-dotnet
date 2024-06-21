using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Mc2.CrudTest.Application2.Services;
using Mc2.CrudTest.Domain2;
using Mc2.CrudTest.Domain2.IRepositories;
using Mc2.CrudTest.Domain2.Models;
using NSubstitute;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.ApplicationServices.Services
{
    [ExcludeFromCodeCoverage]
    public class CustomerServiceTest
    {
        private readonly CustomerService _service;

        public CustomerServiceTest()
        {
            IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();
            ICustomerRepository repository = Substitute.For<ICustomerRepository>();

            _service = new CustomerService(unitOfWork, repository);
        }

        [Fact]
        public async Task GetCustomersListAsync_WhenCall_ReturnsIEnumerableOfCustomers()
        {
            // Act
            var result = await _service.GetCustomersListAsync();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<Customer>>();
        }

        [Fact]
        public async Task GetCustomerByIdAsync_WhenCall_ReturnsCustomer()
        {
            // Assign
            Customer customer = new();
            _service.GetCustomerByIdAsync(1).Returns(customer);

            // Act
            var result = await _service.GetCustomerByIdAsync(1);

            // Assert
            result.Should().Be(customer);
        }

        [Fact]
        public async Task CreateCustomerAsync_WhenCall_ReturnsCustomer()
        {
            // Arrange
            DateTime dateOfBirth = DateTime.Today.AddYears(-20);
            Customer customer = new()
            {
                Id = 0,
                FirstName = "Ramin",
                LastName = "Bateni",
                DateOfBirth = dateOfBirth,
                Email = "a@a.com",
                PhoneNumber = "+989130000000",
                BankAccountNumber = "987654"
            };

            // Act
            CreateOrUpdateResult<Customer> result = await _service.CreateCustomerAsync(customer);

            // Assert
            result.Model.Should().Be(customer);
        }

        [Fact]
        public async Task UpdateCustomerAsync_WhenCall_ReturnsId()
        {
            // Arrange
            DateTime dateOfBirth = DateTime.Today.AddYears(-20);
            Customer customer = new()
            {
                Id = 1,
                FirstName = "Ramin",
                LastName = "Bateni",
                DateOfBirth = dateOfBirth,
                Email = "a@a.com",
                PhoneNumber = "+989130000000",
                BankAccountNumber = "987654"
            };

            // Act
            CreateOrUpdateResult<Customer> result = await _service.UpdateCustomerAsync(customer);

            // Assert
            result.Model.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteCustomerAsync_WhenCall_ReturnsId()
        {
            // Act
            int result = await _service.DeleteCustomerAsync(1);

            // Assert
            result.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task IsSameCustomerExistAsync_WhenCall_ReturnsCustomerId()
        {
            // Act
            int result = await _service.GetCustomerIdOfCustomerAsync("a", "b", DateTime.Now);

            // Assert
            Assert.IsType<int>(result);
        }

        [Fact]
        public async Task IsEmailExistAsync_WhenCall_ReturnsCustomerId()
        {
            // Act
            int result = await _service.GetCustomerIdByEmailAsync("a@a.com");

            // Assert
            Assert.IsType<int>(result);
        }
    }
}
