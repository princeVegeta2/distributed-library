namespace DistributedLibrary.Main.Infrastructure.Webhooks
{
    internal interface IWebhookDispatcher
    {
        Task EnqueueWebhookAsync(string eventType, object payload);
    }
}
