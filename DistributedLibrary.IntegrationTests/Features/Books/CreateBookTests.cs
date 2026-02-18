using DistributedLibrary.IntegrationTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DistributedLibrary.IntegrationTests.Features.Books
{
    public sealed class CreateBookTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public CreateBookTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateBook_ValidRequest_Produces201AndGuid()
        {
            // Arrange
            var authorBody = new
            {
                Name = "Test Author"
            };
            var authorResult = await _client.PostAsJsonAsync("/api/authors/", authorBody);
            var authorJson = JsonDocument.Parse(await authorResult.Content.ReadAsStringAsync());
            Assert.True(authorJson.RootElement.TryGetProperty("id", out var authorIdEl));
            var authorIdString = authorIdEl.GetString();
            Assert.False(string.IsNullOrEmpty(authorIdString));
            Assert.True(Guid.TryParse(authorIdString, out var authorId));

            var bookBody = new
            {
                Title = "Test book",
                PublishedAt = DateTimeOffset.UtcNow.AddYears(-1),
                AuthorId = authorId
            };

            // Act
            var result = await _client.PostAsJsonAsync("/api/books/", bookBody);

            // Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

            var json = JsonDocument.Parse(await result.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("id", out var bookIdEl));
            var bookIdString = bookIdEl.GetString();
            Assert.False(string.IsNullOrEmpty(bookIdString));
            Assert.True(Guid.TryParse(bookIdString, out var _));
        }
    }
}
