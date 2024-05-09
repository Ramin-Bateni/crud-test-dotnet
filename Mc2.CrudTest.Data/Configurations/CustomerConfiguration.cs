using System.Diagnostics.CodeAnalysis;
using Mc2.CrudTest.ApplicationServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mc2.CrudTest.Data.Configurations
{
    [ExcludeFromCodeCoverage]
    class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .ValueGeneratedNever()
                .IsUnicode(false)
                .HasMaxLength(100);
            
            builder.Property(x => x.FirstName)
                .IsUnicode()
                .HasMaxLength(30);
            
            builder.Property(x => x.LastName)
                .IsUnicode(false)
                .HasMaxLength(30);

            builder.Property(x => x.PhoneNumber)
                .IsUnicode(false)
                .HasMaxLength(15);

            builder.Property(x => x.BankAccountNumber)
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}
