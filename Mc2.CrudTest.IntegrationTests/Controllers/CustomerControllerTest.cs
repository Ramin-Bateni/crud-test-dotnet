using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Mc2.CrudTest.Data;
using Mc2.CrudTest.IntegrationTests.Models;
using Mc2.CrudTest.Presentation.Server;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Mc2.CrudTest.IntegrationTests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class CustomerControllerTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public CustomerControllerTest(CustomWebApplicationFactory<Startup> factory)
        {
            var scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://localhost");
        }

        [Fact]
        public async Task GetAll_WhenCall_ReturnsListOfCustomers()
        {
            // Arrange
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            List<Customer>? response = await _httpClient.GetFromJsonAsync<List<Customer>>("Customer/GetAll");

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_WhenCall_ReturnsCustomer()
        {
            // Arrange
            DataGenerator.CleanAndAdd2Customer(_dbContext);
            int id = DataGenerator.CustomerItems.First().Id;

            // Act
            var response = await _httpClient.GetFromJsonAsync<Customer>($"Customer/GetById/{id}");

            // Assert
            response.Should().NotBeNull();
            response?.Should().BeEquivalentTo(DataGenerator.CustomerItems.First());
        }

        [Fact]
        public async Task Create_WhenCall_ReturnsCustomer()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "M",
                Email = "b@b.com",
                PhoneNumber = "09110000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = DateTime.Now.AddYears(-40)
            };
            DataGenerator.CleanCustomers(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Customer/Create", customer);
            Customer responseContent = await response.Content.ReadFromJsonAsync<Customer>();

            // Assert
            responseContent.Should().NotBeNull();
            responseContent?.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public async Task Create_SameCustomerIsExist_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "Ramin",
                LastName = "Bateni",
                Email = "a123@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Customer/Create", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_SameEmailIsExist_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Customer/Create", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Create_EmailIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Customer/Create", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_PhoneNumberIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a@a.com",
                PhoneNumber = "abc",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Customer/Create", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_BankAccountIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "123",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"Customer/Create", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_WhenCall_ReturnsCustomer()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 2,
                FirstName = "Ramin 2",
                LastName = "Bateni 2",
                Email = "abc@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = DateTime.Now.AddYears(-30)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Customer/Update", customer);
            string responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            responseContent.Should().NotBeNull();
            responseContent?.Should().BeEquivalentTo("1"); // Count of updated items
        }

        [Fact]
        public async Task Update_SameCustomerIsExist_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "Ramin",
                LastName = "Bateni",
                Email = "a123@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Customer/Update", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_SameEmailIsExist_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Customer/Update", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Update_EmailIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a",
                PhoneNumber = "09100000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Customer/Update", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_PhoneNumberIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a@a.com",
                PhoneNumber = "abc",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Customer/Update", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Update_BankAccountIsInvalid_ReturnsBadRequest()
        {
            // Arrange
            Customer customer = new()
            {
                Id = 3,
                FirstName = "R",
                LastName = "B",
                Email = "a@a.com",
                PhoneNumber = "09100000000",
                BankAccountNumber = "123",
                DateOfBirth = new DateTime(1990, 1, 1)
            };
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"Customer/Update", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WhenCall_ReturnsCustomer()
        {
            // Arrange
            int id = DataGenerator.CustomerItems.First().Id;
            DataGenerator.CleanAndAdd2Customer(_dbContext);

            // Act
            HttpResponseMessage response = await _httpClient.DeleteAsync($"Customer/Delete/{id}");

            // Assert
            response.EnsureSuccessStatusCode();

            string resultId = await response.Content.ReadAsStringAsync();
            resultId.Should().Be("1"); // count of deleted items
        }
    }
}