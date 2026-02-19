using DistributedLibrary.IntegrationTests.Common; // Import extensions
using DistributedLibrary.IntegrationTests.Infrastructure;
using DistributedLibrary.Main.Features.Authors.GetAuthor;
using System.Net;
using System.Net.Http.Json;

namespace DistributedLibrary.IntegrationTests.Features.Authors
{
    public sealed class GetAuthorTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetAuthorTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAuthor_ValidRequest_Returns200AndAuthorWithBooks()
        {
            // Arrange - Define our expected data
            var expectedAuthorName = "Test Author";

            var book1Title = "Test book";
            var book1Date = DateTimeOffset.UtcNow.AddYears(-1);

            var book2Title = "Second book";
            var book2Date = DateTimeOffset.UtcNow.AddYears(-2);

            // Use Arrangers to populate the DB cleanly
            var authorId = await _client.CreateAuthorAsync(expectedAuthorName);
            await _client.CreateBookAsync(authorId, book1Title, book1Date);
            await _client.CreateBookAsync(authorId, book2Title, book2Date);

            // Act
            var result = await _client.GetAsync($"/api/authors/{authorId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            // Deserialize
            var authorResponse = await result.Content.ReadFromJsonAsync<GetAuthorResponse>();
            Assert.NotNull(authorResponse);

            // Assert Author properties
            Assert.Equal(authorId, authorResponse.Id);
            Assert.Equal(expectedAuthorName, authorResponse.Name);

            // Assert Collection properties
            Assert.NotNull(authorResponse.Books);
            Assert.Equal(2, authorResponse.Books.Count);

            // Assert Book 1 (Using Single() to find it by unique title)
            var responseBook1 = authorResponse.Books.Single(b => b.Title == book1Title);

            var timeDiff1 = (book1Date - responseBook1.PublishedAt).Duration();
            Assert.True(timeDiff1 < TimeSpan.FromSeconds(1),
                $"Expected {book1Date}, but got {responseBook1.PublishedAt}");

            // Assert Book 2
            var responseBook2 = authorResponse.Books.Single(b => b.Title == book2Title);

            var timeDiff2 = (book2Date - responseBook2.PublishedAt).Duration();
            Assert.True(timeDiff2 < TimeSpan.FromSeconds(1),
                $"Expected {book2Date}, but got {responseBook2.PublishedAt}");
        }
    }
}