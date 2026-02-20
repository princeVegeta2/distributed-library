namespace DistributedLibrary.Main.Infrastructure.Webhooks
{
    internal sealed record WebhookMessage(string EventType, object Payload);
}
