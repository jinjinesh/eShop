using System.Data.Common;
using System.Text;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Transporter;

public class RabbitMQMessagePublisher : IDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _exchange;
    private readonly string _exchangeType;

    public RabbitMQMessagePublisher(string hostName, string username, string password, string exchange, string exchangeType, string queueName)
    {
        _factory = new ConnectionFactory() { HostName = hostName, UserName = username, Password = password};
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();

        _exchange = exchange;
        _exchangeType = exchangeType;

        _channel.ExchangeDeclare(exchange: _exchange, type: _exchangeType);

        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        // If you are using a topic exchange, you may want to bind the queue to a routing key
        if (_exchangeType.Equals("topic", StringComparison.OrdinalIgnoreCase))
        {
            _channel.QueueBind(queue: queueName, exchange: _exchange, routingKey: "");
        }

    }

    public void PublishMessage(string message, string routingKey)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(exchange: _exchange, routingKey: routingKey, basicProperties: null, body: body);
        Console.WriteLine($" [x] Sent '{message}' with routing key '{routingKey}'");
    }

    public void ConsumeMessages(string routingKey, Action<string> handleMessage)
    {
        var queueName = _channel.QueueDeclare().QueueName;

        _channel.QueueBind(queue: queueName, exchange: _exchange, routingKey: routingKey);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            handleMessage(message);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}