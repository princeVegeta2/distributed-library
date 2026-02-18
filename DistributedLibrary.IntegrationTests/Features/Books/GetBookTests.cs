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
    public sealed class GetBookTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetBookTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        /// <summary>
        /// Get book test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBook_ValidRequest_Produces200AndBookWithAuthor()
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
            Assert.Equal(HttpStatusCode.Created, bookResult.StatusCode);

            var json = JsonDocument.Parse(await bookResult.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("id", out var bookIdEl));
            var bookIdString = bookIdEl.GetString();
            Assert.False(string.IsNullOrEmpty(bookIdString));
            Assert.True(Guid.TryParse(bookIdString, out var bookId));

            // Act
            var result = await _client.GetAsync($"/api/books/{bookId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            // Deserialize the response into the exact DTO
            var bookResponse = await result.Content.ReadFromJsonAsync<GetBookResponse>();

            Assert.NotNull(bookResponse);

            Assert.Equal(bookId, bookResponse.Id);
            Assert.Equal("Test book", bookResponse.Title);

            Assert.Equal(bookBody.PublishedAt, bookResponse.PublishedAt);

            Assert.NotNull(bookResponse.Author);
            Assert.Equal(authorId, bookResponse.Author.Id);
            Assert.Equal("Test Author", bookResponse.Author.Name);
        }
    }
}
