using DistributedLibrary.IntegrationTests.Infrastructure;
using DistributedLibrary.Main.Features.Authors.GetAuthor;
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
    public sealed class GetAuthorTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GetAuthorTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        /// <summary>
        /// Get an author test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAuthor_ValidRequest_Returns200AndAuthorWithBooks()
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

            var bookBody1 = new
            {
                Title = "Test book",
                PublishedAt = DateTimeOffset.UtcNow.AddYears(-1),
                AuthorId = authorId
            };
            var bookBody2 = new
            {
                Title = "Second book",
                PublishedAt = DateTimeOffset.UtcNow.AddYears(-2),
                AuthorId = authorId
            };
            var bookOneResult = await _client.PostAsJsonAsync("/api/books/", bookBody1);
            Assert.Equal(HttpStatusCode.Created, bookOneResult.StatusCode);
            var bookTwoResult = await _client.PostAsJsonAsync("/api/books/", bookBody2);
            Assert.Equal(HttpStatusCode.Created, bookTwoResult.StatusCode);

            // Act
            var result = await _client.GetAsync($"/api/authors/{authorId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            // Deserialize the response into the exact dto
            // Deserialize the response into the exact dto
            var authorResponse = await result.Content.ReadFromJsonAsync<GetAuthorResponse>();

            Assert.NotNull(authorResponse);

            // Assert same author
            Assert.Equal(authorId, authorResponse.Id);
            Assert.Equal(authorBody.Name, authorResponse.Name);

            // Assert we got exactly 2 books back
            Assert.NotNull(authorResponse.Books);
            Assert.Equal(2, authorResponse.Books.Count);

            // ACT & ASSERT: Find Book 1 by Title and assert its properties
            var responseBook1 = authorResponse.Books.Single(b => b.Title == bookBody1.Title);

            // Use a TimeSpan delta to avoid the DateTime precision loss trap
            var timeDifference1 = (bookBody1.PublishedAt - responseBook1.PublishedAt).Duration();
            Assert.True(timeDifference1 < TimeSpan.FromSeconds(1),
                $"Expected {bookBody1.PublishedAt}, but got {responseBook1.PublishedAt}");

            // ACT & ASSERT: Find Book 2 by Title and assert its properties
            var responseBook2 = authorResponse.Books.Single(b => b.Title == bookBody2.Title);

            var timeDifference2 = (bookBody2.PublishedAt - responseBook2.PublishedAt).Duration();
            Assert.True(timeDifference2 < TimeSpan.FromSeconds(1),
                $"Expected {bookBody2.PublishedAt}, but got {responseBook2.PublishedAt}");
        }
    }
}
