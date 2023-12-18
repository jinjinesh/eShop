using Transporter;

namespace NotificationService2;

public class UpdationEventHandler : BackgroundService
{
    private readonly ILogger<CreationEventHandler> _logger;
    private readonly RabbitMQMessagePublisher _messagePublisher;

    public UpdationEventHandler(ILogger<CreationEventHandler> logger)
    {
        _logger = logger;
        _messagePublisher =
            new RabbitMQMessagePublisher("localhost", "guest", "guest", "orderUpdation", "topic", "updation");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _messagePublisher.ConsumeMessages("updation",
            (message) => _logger.LogInformation($"Received order updation notification message for notification service 2 - {message}"));
        await Task.CompletedTask;
    }
}