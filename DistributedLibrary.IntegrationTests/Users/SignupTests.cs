using DistributedLibrary.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DistributedLibrary.IntegrationTests.Users
{
    public sealed class SignupTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public SignupTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Signup_ValidRequest_Produces201AndGuid()
        {
            // Arrange
            var body = new
            {
                Username = "Test",
                Email = "test@example.com",
                Password = "Password123"
            };

            // Act
            var result = await _client.PostAsJsonAsync("/api/users/sign-up/", body);
            
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
