using DistributedLibrary.IntegrationTests.Common; // Import extensions
using DistributedLibrary.IntegrationTests.Infrastructure;
using System.Net;

namespace DistributedLibrary.IntegrationTests.Features.Authors
{
    public sealed class DeleteAuthorTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public DeleteAuthorTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task DeleteAuthor_ValidRequest_ProducesNoContentAndPersistsDeletion()
        {
            // Arrange - Create a unique author in one line
            var authorId = await _client.CreateAuthorAsync();

            // Act 1: Send the Delete request
            var deleteResult = await _client.DeleteAsync($"/api/authors/{authorId}");

            // Assert 1: Verify the API returns 204 No Content
            Assert.Equal(HttpStatusCode.NoContent, deleteResult.StatusCode);

            // Act 2: Try to fetch the author again to prove they are gone
            var getResult = await _client.GetAsync($"/api/authors/{authorId}");

            // Assert 2: Verify the API returns 404 Not Found
            Assert.Equal(HttpStatusCode.NotFound, getResult.StatusCode);
        }
    }
}