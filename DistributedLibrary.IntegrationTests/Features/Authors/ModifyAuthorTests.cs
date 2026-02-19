using DistributedLibrary.IntegrationTests.Common; // Import your extension methods
using DistributedLibrary.IntegrationTests.Infrastructure;
using DistributedLibrary.Main.Features.Authors.GetAuthor; // Import the DTO namespace
using System.Net;
using System.Net.Http.Json;

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
        public async Task ModifyAuthor_ValidRequest_ProducesNoContentAndPersistsChange()
        {
            // Arrange - Create an author cleanly using the helper
            var authorId = await _client.CreateAuthorAsync();

            var modifyBody = new
            {
                Name = "New Name"
            };

            // Act 1: Send the Patch request
            var patchResult = await _client.PatchAsJsonAsync($"/api/authors/{authorId}", modifyBody);

            // Assert 1: Verify the API returns 204 No Content
            Assert.Equal(HttpStatusCode.NoContent, patchResult.StatusCode);

            // Act 2: Fetch the author to verify the change was persisted to the DB
            var getResult = await _client.GetAsync($"/api/authors/{authorId}");
            Assert.Equal(HttpStatusCode.OK, getResult.StatusCode);

            // Assert 2: Verify the content matches
            var authorResponse = await getResult.Content.ReadFromJsonAsync<GetAuthorResponse>();
            Assert.NotNull(authorResponse);

            Assert.Equal(authorId, authorResponse.Id);
            Assert.Equal(modifyBody.Name, authorResponse.Name);
        }
    }
}