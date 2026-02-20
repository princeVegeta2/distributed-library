
using System.Threading.Channels;

namespace DistributedLibrary.Main.Infrastructure.Webhooks
{
    internal sealed class WebhookDispatcher : IWebhookDispatcher
    {
        private readonly Channel<WebhookMessage> _channel;

        public WebhookDispatcher()
        {
            var options = new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _channel = Channel.CreateBounded<WebhookMessage>(options);
        }

        public async Task EnqueueWebhookAsync(string eventType, object payload)
        {
            var message = new WebhookMessage(eventType, payload);
            await _channel.Writer.WriteAsync(message);
        }

        public ChannelReader<WebhookMessage> Reader => _channel.Reader;
    }
}
