namespace DistributedLibrary.Main.Features.Books.ModifyBook
{
    internal sealed record ModifyBookRequest(string? Title, DateTimeOffset? PublishedAt);
}
