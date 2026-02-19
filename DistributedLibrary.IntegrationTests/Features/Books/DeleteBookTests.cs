using DistributedLibrary.IntegrationTests.Common;
using DistributedLibrary.IntegrationTests.Infrastructure;
using System.Net; // Import your extension methods

namespace DistributedLibrary.IntegrationTests.Features.Books
{
    public sealed class DeleteBookTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DeleteBookTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DeleteBook_ValidRequest_ProducesNoContent()
        {
            // Arrange - Using the extension methods
            var authorId = await _client.CreateAuthorAsync();
            var bookId = await _client.CreateBookAsync(authorId);

            // Act
            var result = await _client.DeleteAsync($"/api/books/{bookId}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task DeleteBook_ValidRequest_PersistsDeletion()
        {
            // Arrange
            var authorId = await _client.CreateAuthorAsync();
            var bookId = await _client.CreateBookAsync(authorId);

            // Act
            await _client.DeleteAsync($"/api/books/{bookId}");

            // Assert
            var getResult = await _client.GetAsync($"/api/books/{bookId}");
            Assert.Equal(HttpStatusCode.NotFound, getResult.StatusCode);
        }
    }
}