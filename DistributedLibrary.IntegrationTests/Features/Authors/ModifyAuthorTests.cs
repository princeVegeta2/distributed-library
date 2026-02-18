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
    public sealed class ModifyAuthorTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ModifyAuthorTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ModifyAuthor_ValidRequest_ProducesNoContent()
        {
            // Arrange
            var authorBody = new
            {
                Name = "Test Author"
            };
            var authorResult = await _client.PostAsJsonAsync("/api/authors/", authorBody);
            Assert.Equal(HttpStatusCode.Created, authorResult.StatusCode);
            var authorJson = JsonDocument.Parse(await  authorResult.Content.ReadAsStringAsync());
            Assert.True(authorJson.RootElement.TryGetProperty("id", out var authorIdEl));
            var authorIdString = authorIdEl.GetString();
            Assert.False(string.IsNullOrEmpty(authorIdString));
            Assert.True(Guid.TryParse(authorIdString, out var authorId));

            var modifyBody = new
            {
                Name = "New Name"
            };

            // Act
            var result = await _client.PatchAsJsonAsync($"/api/authors/{authorId}", modifyBody);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task ModifyAuthor_ValidRequest_PersistsChanges()
        {
            // Arrange
            var authorBody = new
            {
                Name = "Test Author"
            };
            var authorResult = await _client.PostAsJsonAsync("/api/authors/", authorBody);
            Assert.Equal(HttpStatusCode.Created, authorResult.StatusCode);
            var authorJson = JsonDocument.Parse(await authorResult.Content.ReadAsStringAsync());
            Assert.True(authorJson.RootElement.TryGetProperty("id", out var authorIdEl));
            var authorIdString = authorIdEl.GetString();
            Assert.False(string.IsNullOrEmpty(authorIdString));
            Assert.True(Guid.TryParse(authorIdString, out var authorId));

            var modifyBody = new
            {
                Name = "New Name"
            };

            // Act
            var result = await _client.PatchAsJsonAsync($"/api/authors/{authorId}", modifyBody);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            var getResult = await _client.GetAsync($"/api/authors/{authorId}");
            Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);
            var json = JsonDocument.Parse(await  getResult.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("name", out var authorNameEl));
            var authorName = authorNameEl.GetString();
            Assert.Equal(modifyBody.Name, authorName);
        }
    }
}
