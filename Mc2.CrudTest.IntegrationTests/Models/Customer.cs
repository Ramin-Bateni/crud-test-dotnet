using System;
using System.Diagnostics.CodeAnalysis;

namespace Mc2.CrudTest.IntegrationTests.Models
{
    [ExcludeFromCodeCoverage]
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string BankAccountNumber { get; set; }
    }
}
