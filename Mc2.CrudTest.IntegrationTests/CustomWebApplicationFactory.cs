using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mc2.CrudTest.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Mc2.CrudTest.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public CustomWebApplicationFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove the existing context configuration
                ServiceDescriptor descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TestDB"));
                
                IServiceProvider serviceProvider = services.BuildServiceProvider().GetRequiredService<IServiceProvider>();

                DataGenerator.Initialize(serviceProvider);
            });
        }
    }
}