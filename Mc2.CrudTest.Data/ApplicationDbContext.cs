using Mc2.CrudTest.Data.Configurations;
using Mc2.CrudTest.Domain2.Models;
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
