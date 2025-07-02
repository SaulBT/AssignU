using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ServicioTareas.Config;

public class RpcClientRabbitMQ : IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private string _replyQueueName;
    private AsyncEventingBasicConsumer _consumer;
    private string _correlationId;
    private TaskCompletionSource<string> _tcsResponse;
    private readonly string _rabbitMqHostDevelopment = "localhost";
    private readonly string _rabbitMqHostProduction = "rabbitmq";

    public async Task InicializarCliente()
    {
        var factory = new ConnectionFactory() { HostName = _rabbitMqHostProduction };
        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        _replyQueueName = await _channel.QueueDeclareAsync(queue: "", durable: false, exclusive: true, autoDelete: true);
        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.ReceivedAsync += OnResponseReceived;

        await _channel.BasicConsumeAsync(queue: _replyQueueName, autoAck: true, consumer: _consumer);
    }

    private Task OnResponseReceived(object sender, BasicDeliverEventArgs ea)
    {
        if (ea.BasicProperties.CorrelationId == _correlationId)
        {
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            _tcsResponse.TrySetResult(response);
        }

        return Task.CompletedTask;
    }

    public async Task<string> CallAsync(string queueName, string message, int timeoutMs = 10000)
    {
        _correlationId = Guid.NewGuid().ToString();
        _tcsResponse = new TaskCompletionSource<string>();

        var props = new BasicProperties
        {
            CorrelationId = _correlationId,
            ReplyTo = _replyQueueName
        };

        var messageBytes = Encoding.UTF8.GetBytes(message);
        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: true,
            basicProperties: props,
            body: messageBytes
        );

        using var cts = new CancellationTokenSource(timeoutMs);
        using (cts.Token.Register(() => _tcsResponse.TrySetCanceled()))
        {
            try
            {
                return await _tcsResponse.Task.ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                throw new TimeoutException("Timeout esperando respuesta RPC de RabbitMQ");
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
            await _channel.CloseAsync();

        if (_connection != null)
            await _connection.CloseAsync();
    }
}
