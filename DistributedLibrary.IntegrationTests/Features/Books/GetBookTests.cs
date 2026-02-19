using DistributedLibrary.IntegrationTests.Common; 
using DistributedLibrary.IntegrationTests.Infrastructure;
using DistributedLibrary.Main.Features.Books.GetBook;
using System.Net;
using System.Net.Http.Json;

namespace DistributedLibrary.IntegrationTests.Features.Books
{
    public sealed class GetBookTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetBookTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetBook_ValidRequest_Produces200AndBookWithAuthor()
        {
            // Arrange
            var expectedAuthorName = "Test Author";
            var expectedBookTitle = "Test book";
            var expectedPublishedAt = DateTimeOffset.UtcNow.AddYears(-1);

            // Our shiny new Arrangers in action
            var authorId = await _client.CreateAuthorAsync(expectedAuthorName);
            var bookId = await _client.CreateBookAsync(authorId, expectedBookTitle, expectedPublishedAt);

            // Act
            var result = await _client.GetAsync($"/api/books/{bookId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            // Deserialize the response into the exact DTO
            var bookResponse = await result.Content.ReadFromJsonAsync<GetBookResponse>();
            Assert.NotNull(bookResponse);

            // Assert Book fields
            Assert.Equal(bookId, bookResponse.Id);
            Assert.Equal(expectedBookTitle, bookResponse.Title);

            // Using the TimeSpan trick to prevent flaky DateTime precision errors
            var timeDifference = (expectedPublishedAt - bookResponse.PublishedAt).Duration();
            Assert.True(timeDifference < TimeSpan.FromSeconds(1), 
                $"Expected {expectedPublishedAt}, but got {bookResponse.PublishedAt}");

            // Assert Author fields
            Assert.NotNull(bookResponse.Author);
            Assert.Equal(authorId, bookResponse.Author.Id);
            Assert.Equal(expectedAuthorName, bookResponse.Author.Name);
        }
    }
}