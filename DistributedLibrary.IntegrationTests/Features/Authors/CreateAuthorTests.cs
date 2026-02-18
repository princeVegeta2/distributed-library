using DistributedLibrary.IntegrationTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DistributedLibrary.IntegrationTests.Features.Authors
{
    public sealed class CreateAuthorTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CreateAuthorTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateAuthor_ValidRequest_Produces201AndGuid()
        {
            // Arrange
            var body = new
            {
                Name = "Test Author"
            };

            // Act
            var result = await _client.PostAsJsonAsync("/api/authors/", body);

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

            var json = JsonDocument.Parse(await result.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("id", out var idEl));

            var idString = idEl.GetString();
            Assert.False(string.IsNullOrEmpty(idString));
            Assert.True(Guid.TryParse(idString, out var _));
        }
    }
}
