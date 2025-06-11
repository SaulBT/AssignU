using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ServicioTareas.Data.DTOs;

namespace ServicioTareas.Services;

public class RabbitMQPublisher
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    public RabbitMQPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }

    public async Task PublicarCuestionarioAsync(CuestionarioDTO cuestionario)
    {
        var json = JsonSerializer.Serialize(cuestionario);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.QueueDeclareAsync(
            queue: "cuestionarios_creacion",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: "cuestionarios_creacion",
            mandatory: true,
            body: body
        );
    }
}
