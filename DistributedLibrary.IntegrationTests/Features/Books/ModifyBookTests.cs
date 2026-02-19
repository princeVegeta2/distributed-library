using DistributedLibrary.IntegrationTests.Common; // Brings in our extension methods
using DistributedLibrary.IntegrationTests.Infrastructure;
using DistributedLibrary.Main.Features.Books.GetBook;
using System.Net;
using System.Net.Http.Json;

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
            // Arrange - Lightning fast setup using our Arrangers
            var authorId = await _client.CreateAuthorAsync();
            var bookId = await _client.CreateBookAsync(authorId);

            var patchBody = new
            {
                Title = "New Title",
                PublishedAt = DateTimeOffset.UtcNow.AddYears(-5)
            };

            // Act 1: Send the Patch
            var result = await _client.PatchAsJsonAsync($"/api/books/{bookId}", patchBody);

            // Assert 1: Verify 204 No Content
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            // Act 2 & Assert 2: Fetch the book to prove the DB saved it
            var getResult = await _client.GetAsync($"/api/books/{bookId}");
            Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);

            var bookResponse = await getResult.Content.ReadFromJsonAsync<GetBookResponse>();
            Assert.NotNull(bookResponse);

            // Assert the modified fields match what we sent
            Assert.Equal(patchBody.Title, bookResponse.Title);

            // Safe DateTime comparison
            var timeDifference = (patchBody.PublishedAt - bookResponse.PublishedAt).Duration();
            Assert.True(timeDifference < TimeSpan.FromSeconds(1),
                $"Expected {patchBody.PublishedAt}, but got {bookResponse.PublishedAt}");
        }
    }
}