using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client.Events;

namespace SongService.Messaging;

public class EventListener : IDisposable, IEventListener
{
    private readonly IConnection _connection;

    private readonly ILogger _logger;

    public EventListener(IReadOnlyDictionary<string, string> envStore, ILogger<EventListener> logger)
    {
        _logger = logger;
        var hostname = envStore["RABBITMQ_HOSTNAME"];
        var username = envStore["RABBITMQ_USERNAME"];
        var password = envStore["RABBITMQ_PASSWORD"];

        var factory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };

        while (true)
        {
            try
            {
                _connection = factory.CreateConnection();
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                logger.LogInformation("Retrying to connect to RabbitMQ in 5 seconds...");
                Thread.Sleep(5000);
            }
        }
    }

    public void Subscribe<T>(string topic, Action<T> handler)
    {
        var channel = _connection.CreateModel();

        channel.QueueDeclare(
            queue: topic,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var data = JsonSerializer.Deserialize<T>(json);
            _logger.LogInformation("Received message: {}", data);
            handler(data);
        };

        channel.BasicConsume(
            queue: topic,
            autoAck: true,
            consumer: consumer
        );
    }

    public void Dispose()
    {
        _connection.Close();
        GC.SuppressFinalize(this);
    }
}