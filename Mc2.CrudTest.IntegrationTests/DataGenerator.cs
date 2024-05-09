using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mc2.CrudTest.ApplicationServices.Models;
using Mc2.CrudTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mc2.CrudTest.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class DataGenerator
    {
        public static readonly Customer[] CustomerItems = new Customer[]
        {
            new() {
                Id = 1,
                FirstName = "Michael",
                LastName = "Bilic",
                PhoneNumber = "09120000000",
                Email = "m@m.com",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1995,1,1),
            },new()
            {
                Id = 2,
                FirstName = "Ramin",
                LastName = "Bateni",
                Email = "a@a.com",
                PhoneNumber = "09130000000",
                BankAccountNumber = "0123456789",
                DateOfBirth = new DateTime(1990,1,1)
            }
        };
        
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using ApplicationDbContext context = new(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            CleanCustomers(context);
        }

        public static void CleanCustomers(ApplicationDbContext dbContext)
        {
            var allCustomers= dbContext.Customer.ToList();
            dbContext.Customer.RemoveRange(allCustomers);
        }

        public static void CleanAndAdd2Customer(ApplicationDbContext dbContext)
        {
            CleanCustomers(dbContext);

            dbContext.Customer.AddRange(CustomerItems);

            dbContext.SaveChanges();
        }
    }
}
