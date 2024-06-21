using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BoDi;
using FluentAssertions;
using Mc2.CrudTest.Application2.Customers.Commands;
using Mc2.CrudTest.Application2.Customers.Queries;
using Mc2.CrudTest.Domain2.Enums;
using Mc2.CrudTest.Domain2.Models;
using Mc2.CrudTest.Shared.Utilities;
using MediatR;
using NSubstitute;
using TechTalk.SpecFlow;
using Xunit;

namespace Mc2.CrudTest.SpecFlowTests.Steps
{
    [Binding]
    public class UserCanCreateUpdateReadAndDeleteCustomersSteps
    {
        private readonly IMediator _mediator;
        private readonly ScenarioContext _scenarioContext;

        public UserCanCreateUpdateReadAndDeleteCustomersSteps(ScenarioContext scenarioContext)
        {
            _mediator = Substitute.For<IMediator>();
            _scenarioContext = scenarioContext;

            // Create an empty list of customers
            _scenarioContext["Customers"] = new List<Customer>();

            // Create an empty list of customers
            _scenarioContext["ToReturnErrorCodes"] = new List<int>();
        }

        [Given(@"platform support following error codes")]
        public void GivenPlatformSupportFollowingErrorCodes(Table table)
        {
            _scenarioContext["Errors"] = new Dictionary<int, string>()
            {
                { (int)CustomerCrudErrors.InvalidEmail, "Invalid Email" },
                { (int)CustomerCrudErrors.InvalidPhoneNumber, "Invalid PhoneNumber" },
                { (int)CustomerCrudErrors.InvalidBankAccountNumber, "Invalid Bank Account Number" },
                { (int)CustomerCrudErrors.DuplicatedFirstNameLastnamePhoneNo, "Duplicated FirstName, Lastname in data store" },
                { (int)CustomerCrudErrors.DuplicatedEmail, "Duplicated Email" }
            };
        }
        
        [Given(@"platform has ""(./*)"" records of customers")]
        public void GivenPlatformHasRecordsOfCustomers(int recordsCount)
        {
            if (recordsCount == 0)
            {
                // Create an empty list of customers
                _scenarioContext["Customers"] = new List<Customer>();
            }
            else
            {
                // TODO: For more than 0 records we need to fill the customers list with valid dummy data
                throw new NotImplementedException();
            }
        }
        
        [When(@"user send following request to create a new customer")]
        public async Task WhenUserSendFollowingRequestToCreateANewCustomer(Table table)
        {
            // ####### Arrange

            // Clean the existing errors in the _scenarioContext
            CleanErrorsToShowInContext();

            TableRow? record = table.Rows[0];

            Customer customer = new()
            {
                Id = 1,
                FirstName = record["Firstname"],
                LastName = record["Lastname"],
                DateOfBirth = Convert.ToDateTime(record["DateOfBirth"]),
                PhoneNumber = record["PhoneNumber"],
                Email = record["Email"],
                BankAccountNumber = record["BankAccountNumber"],
            };

            int countOfErrors = 0;
            bool isEmailValid = Validator.EmailIsValid(customer.Email);
            bool isPhoneValid = Validator.PhoneIsValid(customer.PhoneNumber);
            bool isBankAccountIsValid = Validator.BankAccountIsValid(customer.BankAccountNumber);
            bool isDuplicatedFirstNameLastnamePhoneNo = IsDuplicatedFirstNameLastnamePhoneNo(customer.FirstName, customer.LastName, customer.DateOfBirth);
            bool isDuplicatedEmail = IsDuplicatedEmail(customer.Email);

            if (!isEmailValid)
            {
                countOfErrors++;
                AddErrorToShowInContext((int)CustomerCrudErrors.InvalidEmail); // 101
            }

            if (!isPhoneValid)
            {
                countOfErrors++;
                AddErrorToShowInContext((int)CustomerCrudErrors.InvalidPhoneNumber); // 102
            }

            if (!isBankAccountIsValid)
            {
                countOfErrors++;
                AddErrorToShowInContext((int)CustomerCrudErrors.InvalidBankAccountNumber); // 103
            }

            if (isDuplicatedFirstNameLastnamePhoneNo)
            {
                countOfErrors++;
                AddErrorToShowInContext((int)CustomerCrudErrors.DuplicatedFirstNameLastnamePhoneNo); // 201
            }

            if (isDuplicatedEmail)
            {
                countOfErrors++;
                AddErrorToShowInContext((int)CustomerCrudErrors.DuplicatedEmail); // 202
            }

            CreateOrUpdateResult<Customer> expectedResult;
            int countOfErrorsToShow = GetErrorsToShowFromContext().Count;

            // If we have any errors
            if (countOfErrorsToShow > 0)
            {
                // So we expect to see the error codes in the result
                expectedResult = CreateOrUpdateResult<Customer>.AddErrors(GetErrorsToShowFromContext());
            }
            else
            {
                expectedResult = CreateOrUpdateResult<Customer>.SetModel(customer);
            }

            CreateCustomerCommand createCustomerCommand = new()
            {
                FirstName = customer.FirstName,
                Lastname = customer.LastName,
                Email = customer.Email,
                DateOfBirth = customer.DateOfBirth,
                PhoneNumber = customer.PhoneNumber,
                BankAccountNumber = customer.BankAccountNumber
            };

            // Setup Mediator Mock
            _mediator.Send(createCustomerCommand).Returns(expectedResult);


            // ####### Act
            if (countOfErrorsToShow == 0)
            {
                // Add the Customer to the _scenarioContext
                ((List<Customer>)_scenarioContext["Customers"]).Add(customer);
            }

            // Create the customer record with mock mediator
            CreateOrUpdateResult<Customer> result = await _mediator.Send(createCustomerCommand);


            // ####### Assert
            // If we have any errors
            if (countOfErrorsToShow > 0)
            {
                result.HasError.Should().BeTrue();
                result.Errors.Should().HaveCount(countOfErrors);

                if (!isEmailValid)
                {
                    result.Errors.Should().Contain((int)CustomerCrudErrors.InvalidEmail); // 101
                }

                if (!isPhoneValid)
                {
                    result.Errors.Should().Contain((int)CustomerCrudErrors.InvalidPhoneNumber); // 102
                }

                if (!isBankAccountIsValid)
                {
                    result.Errors.Should().Contain((int)CustomerCrudErrors.InvalidBankAccountNumber); // 103
                }

                if (isDuplicatedFirstNameLastnamePhoneNo)
                {
                    result.Errors.Should().Contain((int)CustomerCrudErrors.DuplicatedFirstNameLastnamePhoneNo); // 201
                }

                if (isDuplicatedEmail)
                {
                    result.Errors.Should().Contain((int)CustomerCrudErrors.DuplicatedEmail); // 202
                }
            }
            // When we have no errors
            else
            {
                result.IsOk.Should().BeTrue();
                result.Errors.Should().HaveCount(0);
                result.Model.Should().BeEquivalentTo(customer);
            }
        }

        [When(@"user send following request to update a customer with email ""(.*)"" with following information")]
        public async Task WhenUserSendFollowingRequestToUpdateACustomerWithEmailWithFollowingInformation(string oldEmail, Table table)
        {
            // Arrange
            TableRow? record = table.Rows[0];
            Customer? toUpdateCustomer = GetCustomerFromContextBy(oldEmail);

            if (toUpdateCustomer == null)
            {
                // TODO : After this case added to SpecFlow Scenario, we should implement it here
                return;
            }
            
            toUpdateCustomer.FirstName = record["Firstname"];
            toUpdateCustomer.LastName = record["Lastname"];
            toUpdateCustomer.DateOfBirth = Convert.ToDateTime(record["DateOfBirth"]);
            toUpdateCustomer.PhoneNumber = record["PhoneNumber"];
            toUpdateCustomer.Email = record["Email"];
            toUpdateCustomer.BankAccountNumber = record["BankAccountNumber"];
        

            UpdateCustomerCommand updateCustomerCommand = new()
            {
                FirstName = toUpdateCustomer.FirstName,
                LastName = toUpdateCustomer.LastName,
                Email = toUpdateCustomer.Email,
                DateOfBirth = toUpdateCustomer.DateOfBirth,
                PhoneNumber = toUpdateCustomer.PhoneNumber,
                BankAccountNumber = toUpdateCustomer.BankAccountNumber
            };

            var expectedResult = CreateOrUpdateResult<Customer>.SetModel(toUpdateCustomer);

            // Setup Mediator Mock
            _mediator.Send(updateCustomerCommand).Returns(expectedResult);

            // Act
            // Update the customer record with mock mediator
            CreateOrUpdateResult<Customer> result = await _mediator.Send(updateCustomerCommand);

            // Assert
            result.IsOk.Should().BeTrue();
            result.Errors.Should().BeEmpty();
            result.Model.Should().BeEquivalentTo(toUpdateCustomer);
        }

        [When(@"user send request to delete a customer with email ""(.*)""")]
        public async Task WhenUserSendRequestToDeleteACustomerWithEmail(string email)
        {
            // Arrange
            Customer? toDeleteCustomer = GetCustomerFromContextBy(email);
            List<Customer> customers = GetCustomersFromContext();

            if (toDeleteCustomer == null)
            {
                // TODO : After this case added to SpecFlow Scenario, we should implement it here
                return;
            }

            DeleteCustomerCommand deleteCustomerCommand = new()
            {
                Id = toDeleteCustomer.Id
            };

            // Setup Mediator Mock
            _mediator.Send(deleteCustomerCommand).Returns(1);

            // Act
            // Delete from context
            customers.Remove(toDeleteCustomer);

            // Delete the customer record with mock mediator
            int result = await _mediator.Send(deleteCustomerCommand);

            // Assert
            result.Should().Be(1);
        }
        
        [Then(@"administrator can query and get ""(.*)"" record of user with below information")]
        public async Task ThenAdministratorCanQueryAndGetRecordOfUserWithBelowInformation(int recordsCount, Table table)
        {
            // Arrange
            TableRow? record = table.Rows[0];

            Customer toFindCustomer = new Customer()
            {
                FirstName = record["Firstname"],
                LastName = record["Lastname"],
                DateOfBirth = Convert.ToDateTime(record["DateOfBirth"]),
                PhoneNumber = record["PhoneNumber"],
                Email = record["Email"],
                BankAccountNumber = record["BankAccountNumber"]
            };

            List<Customer> customers = GetCustomersFromContextBy(
                toFindCustomer.FirstName,
                toFindCustomer.LastName,
                toFindCustomer.DateOfBirth,
                toFindCustomer.PhoneNumber,
                toFindCustomer.Email,
                toFindCustomer.BankAccountNumber
            );

            GetCustomersByInfoQuery getCustomersByInfoQuery = new() { Customer = toFindCustomer };

            // Setup Mediator Mock
            _mediator.Send(getCustomersByInfoQuery).Returns(customers);

            // Act
            // Get customer records with mock mediator
            IEnumerable<Customer> result = await _mediator.Send(getCustomersByInfoQuery);

            // Assert
            customers.Should().HaveCount(recordsCount);
            result.Should().HaveCount(recordsCount);
        }
        
        [Then(@"user will receive following error codes")]
        public void ThenUserWillReceiveFollowingErrorCodes(Table table)
        {
            // Arrange
            List<int> inputErrors = table.Rows.Select(row => int.Parse(row.Values.First())).ToList();

            // Act
            List<int> errorsInContextToShow = GetErrorsToShowFromContext();

            // Assert
            errorsInContextToShow.Should().BeEquivalentTo(inputErrors);
        }
        
        [Then(@"administrator query to get all customers and get ""(.*)"" records of customers")]
        public async Task ThenAdministratorQueryToGetAllCustomersAndGetRecordsOfCustomers(int recordsCount)
        {
            // Arrange
            List<Customer> customersInContext = GetCustomersFromContext();

            GetAllCustomersQuery getAllCustomersQuery = new();

            // Setup Mediator Mock
            _mediator.Send(getAllCustomersQuery).Returns(customersInContext);

            // Act
            // Get all customers with mock mediator
            IEnumerable<Customer> result = await _mediator.Send(getAllCustomersQuery);

            // Assert
            customersInContext.Should().HaveCount(recordsCount);
            result.Should().HaveCount(recordsCount);
        }


        private List<Customer> GetCustomersFromContext()
        {
            return ((List<Customer>)_scenarioContext["Customers"]);
        }

        private Customer? GetCustomerFromContextBy(string email)
        {
            return ((List<Customer?>)_scenarioContext["Customers"])?
                   .FirstOrDefault(x => string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase));
        }

        private List<Customer> GetCustomersFromContextBy(string firstName, string lastName, DateTime dateOfBirth, string phoneNo , string email, string bankAccountNo)
        {
            return ((List<Customer>)_scenarioContext["Customers"])
                .Where(x =>
                    string.Equals(x.FirstName, firstName, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.LastName, lastName, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.Email, email, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.PhoneNumber, phoneNo, StringComparison.CurrentCultureIgnoreCase) &&
                    string.Equals(x.BankAccountNumber, bankAccountNo, StringComparison.CurrentCultureIgnoreCase) &&
                    x.DateOfBirth == dateOfBirth
                ).ToList();
        }

        private bool IsDuplicatedFirstNameLastnamePhoneNo(string firstName, string lastName, DateTime dateOfBirth)
        {
            return ((List<Customer>)_scenarioContext["Customers"])
                .Any(x => x.FirstName.ToLower() == firstName?.ToLower() &&
                          x.LastName.ToLower() == lastName?.ToLower() &&
                          x.DateOfBirth == dateOfBirth);
        }

        private bool IsDuplicatedEmail(string email)
        {
            return ((List<Customer>)_scenarioContext["Customers"]).Any(x => x.Email.ToLower() == email?.ToLower());
        }

        private void CleanErrorsToShowInContext()
        {
            ((List<int>)_scenarioContext["ToReturnErrorCodes"]).Clear();
        }

        private void AddErrorToShowInContext(int error)
        {
            ((List<int>)_scenarioContext["ToReturnErrorCodes"]).Add(error);
        }

        private List<int> GetErrorsToShowFromContext()
        {
            return ((List<int>)_scenarioContext["ToReturnErrorCodes"]);
        }
    }
}
