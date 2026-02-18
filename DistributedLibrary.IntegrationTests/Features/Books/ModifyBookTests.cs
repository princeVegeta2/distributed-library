using DistributedLibrary.IntegrationTests.Infrastructure;
using DistributedLibrary.Main.Features.Books.GetBook;
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
    public sealed class ModifyBookTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ModifyBookTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ModifyBook_ValidRequestAllFields_ProducesNoContentAndPersistsChange()
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
            var bookResult = await _client.PostAsJsonAsync("/api/books/", bookBody);
            var bookJson = JsonDocument.Parse(await  bookResult.Content.ReadAsStringAsync());
            Assert.True(bookJson.RootElement.TryGetProperty("id", out var bookIdEl));
            var bookIdString = bookIdEl.GetString();    
            Assert.False(string.IsNullOrEmpty(bookIdString));
            Assert.True(Guid.TryParse(bookIdString, out var bookId));

            var body = new
            {
                Title = "New Title",
                PublishedAt = DateTimeOffset.UtcNow.AddYears(-5)
            };

            // Act
            var result = await _client.PatchAsJsonAsync($"/api/books/{bookId}", body);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            var getResult = await _client.GetAsync($"/api/books/{bookId}");
            Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);

            var bookResponse = await getResult.Content.ReadFromJsonAsync<GetBookResponse>();
            Assert.NotNull(bookResponse);

            Assert.Equal(bookResponse.Title, body.Title);
            Assert.Equal(bookResponse.PublishedAt, body.PublishedAt);
        }
    }
}
