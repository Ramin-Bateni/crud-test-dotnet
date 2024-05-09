using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mc2.CrudTest.Presentation.Server;
using Xunit;

namespace Mc2.CrudTest.IntegrationTests
{
    [ExcludeFromCodeCoverage]
    public class HealthCheckTest:IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;

        public HealthCheckTest(CustomWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheck_ReturnsOk()
        {
           HttpResponseMessage response= await _httpClient.GetAsync("/HealthCheck");

           response.EnsureSuccessStatusCode();
           response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
