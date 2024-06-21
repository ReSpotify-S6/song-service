using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace SongService.Messaging;

public class EventListener : IDisposable, IEventListener
{
    private readonly IConnection _connection;

    private readonly IReadOnlyDictionary<string, string> _envStore;

    public EventListener(IReadOnlyDictionary<string, string> envStore)
    {
        var hostname = envStore["RABBITMQ_HOSTNAME"];
        var username = envStore["RABBITMQ_USERNAME"];
        var password = envStore["RABBITMQ_PASSWORD"];

        var factory = new ConnectionFactory
        {
            HostName = hostname,
            UserName = username,
            Password = password
        };

        _connection = factory.CreateConnection();
        _envStore = envStore;
    }
    /*
    public void Subscribe<T>(string topic, Action<T> handler)
    {
        var channel = _connection.CreateModel();

        channel.ExchangeDeclare(exchange: exchange, type: ExchangeType.Direct, durable: true, autoDelete: false);
        channel.QueueDeclare(queue, exclusive: false, autoDelete: false);
        channel.QueueBind(exchange: exchange, routingKey: topic, queue: queue);

        channel.BasicQos(0, 1, false);


        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var data = JsonSerializer.Deserialize<T>(message);

            handler.Invoke(data);
        };

        channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
    }
    */
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _connection.Close();
    }
}