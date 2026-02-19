using System.Net.Http.Json;
using System.Text.Json;

namespace DistributedLibrary.IntegrationTests.Common
{
    internal static class Arrangers
    {
        // Extension method for creating an Author
        public static async Task<Guid> CreateAuthorAsync(this HttpClient client, string? name = null)
        {
            // Use a unique name if none is provided to avoid 409 Conflicts
            var uniqueName = name ?? $"Test Author {Guid.NewGuid()}";
            var body = new { Name = uniqueName };

            var response = await client.PostAsJsonAsync("/api/authors/", body);
            response.EnsureSuccessStatusCode();

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return Guid.Parse(json.RootElement.GetProperty("id").GetString()!);
        }

        // Extension method for creating a 
        public static async Task<Guid> CreateBookAsync(
            this HttpClient client,
            Guid authorId,
            string? title = null,
            DateTimeOffset? publishedAt = null) // <-- Added this
        {
            var uniqueTitle = title ?? $"Test Book {Guid.NewGuid()}";
            var date = publishedAt ?? DateTimeOffset.UtcNow; // <-- Use provided date or default

            var body = new
            {
                Title = uniqueTitle,
                PublishedAt = date,
                AuthorId = authorId
            };

            var response = await client.PostAsJsonAsync("/api/books/", body);
            response.EnsureSuccessStatusCode();

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return Guid.Parse(json.RootElement.GetProperty("id").GetString()!);
        }
    }
}