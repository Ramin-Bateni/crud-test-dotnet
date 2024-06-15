using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mc2.CrudTest.Data;
using Mc2.CrudTest.Data.Repositories;
using Mc2.CrudTest.Domain2.IRepositories;
using Mc2.CrudTest.Domain2.Models;
using Microsoft.EntityFrameworkCore;
using MockQueryable.NSubstitute;
using NSubstitute;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Mc2.CrudTest.AcceptanceTests.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class CustomerRepositoryTest
    {
        private readonly ICustomerRepository _repo;
        private readonly ApplicationDbContext dbContext;

        public CustomerRepositoryTest()
        {
            dbContext = Substitute.For<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            DbSet<Customer> customerDbSet = new List<Customer>()
            {
                new()
                {
                    Id = 1,
                    FirstName = "Ramin",
                    LastName = "Bateni",
                    Email = "a@a.com",
                    DateOfBirth = DateTime.Now,
                    PhoneNumber = "09130000000",
                    BankAccountNumber = "987654"
                }
            }.AsQueryable().BuildMockDbSet();
            
            dbContext.Customer=customerDbSet;

            _repo = new CustomerRepository(dbContext);
        }

        [Fact]
        public async Task GetCustomersListAsync_WhenCall_ReturnsIEnumerableOfCustomers()
        {
            // Act
            var result = await _repo.GetCustomersListAsync();

            // Assert
            result.Should().BeAssignableTo<IEnumerable<Customer>>();
        }

        [Fact]
        public async Task GetCustomerByIdAsync_WhenCall_ReturnsCustomer()
        {
            // Act
            Customer result = await _repo.GetCustomerByIdAsync(1);

            // Assert
            result.Should().BeOfType<Customer>();
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
            Customer result = await _repo.CreateCustomerAsync(customer);

            // Assert
            result.Should().Be(customer);
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
            int result = await _repo.UpdateCustomerAsync(customer);

            // Assert
            result.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task DeleteCustomerAsync_WhenCall_ReturnsId()
        {
            // Act
            int result = await _repo.DeleteCustomerAsync(1);

            // Assert
            result.Should().BeOfType(typeof(int));
        }

        [Fact]
        public async Task IsSameCustomerExistAsync_WhenCall_ReturnsCustomerId()
        {
            // Act
            int result = await _repo.GetCustomerIdOfCustomerAsync("a","b",DateTime.Now);

            // Assert
            Assert.IsInstanceOfType(result,typeof(int));
        }

        [Fact]
        public async Task IsEmailExistAsync_WhenCall_ReturnsCustomerId()
        {
            // Act
            int result = await _repo.GetCustomerIdByEmailAsync("a@a.com");

            // Assert
            Assert.IsInstanceOfType(result, typeof(int));
        }
    }
}
