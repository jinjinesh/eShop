using Transporter;

namespace NotificationService2
{
    public class CreationEventHandler : BackgroundService
    {
        private readonly ILogger<CreationEventHandler> _logger;
        private readonly RabbitMQMessagePublisher _messagePublisher;

        public CreationEventHandler(ILogger<CreationEventHandler> logger)
        {
            _logger = logger;
            _messagePublisher =
                new RabbitMQMessagePublisher("localhost", "guest", "guest", "orderCreation", "fanout", "creation");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _messagePublisher.ConsumeMessages("creation",
                (message) => _logger.LogInformation($"Received order creation notification message for notification service 2 - {message}"));
            await Task.CompletedTask;
        }
    }
}
