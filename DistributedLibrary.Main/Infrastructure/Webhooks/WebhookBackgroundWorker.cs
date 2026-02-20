namespace DistributedLibrary.Main.Infrastructure.Webhooks
{
    internal sealed class WebhookBackgroundWorker : BackgroundService
    {
        private readonly WebhookDispatcher _dispatcher;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WebhookBackgroundWorker> _logger;

        public WebhookBackgroundWorker(
            WebhookDispatcher dispatcher,
            IHttpClientFactory httpClientFactory,
            ILogger<WebhookBackgroundWorker> logger)
        {
            _dispatcher = dispatcher;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var message in _dispatcher.Reader.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await SendWebhookAsync(message, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process webhook for {EventType}", message.EventType);
                }
            }
        }

        public async Task SendWebhookAsync(WebhookMessage message, CancellationToken ct)
        {
            var client = _httpClientFactory.CreateClient("LedgerClient");

            var webhookPayload = new
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTimeOffset.UtcNow,
                Type = message.EventType,
                Data = message.Payload
            };

            var response = await client.PostAsJsonAsync("/api/webhooks/listener", webhookPayload, ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Webhook failed: {StatusCode}", response.StatusCode);
            }
            else
            {
                _logger.LogInformation("Webhook sent: {EventType}", message.EventType);
            }
        }
    }
}
