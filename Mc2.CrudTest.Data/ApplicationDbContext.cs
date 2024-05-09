using Mc2.CrudTest.ApplicationServices.Models;
using Mc2.CrudTest.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Mc2.CrudTest.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Customer> Customer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        }
    }
}
