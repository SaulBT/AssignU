using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using ServicioClases.Services.Interfaces;
using ServicioClases.Data.DTOs.RPC;

namespace ServicioClases.Config
{
    public class RpcServerRabbitMQ
    {
        private IConnection _connection;
        private IChannel _channel;
        private string _queueName;
        private IServicioClase _ServicioClase;
        private readonly string _rabbitMqHostDevelopment = "localhost";
        private readonly string _rabbitMqHostProduction = "rabbitmq";

        public async Task InicializarServidor(IServicioClase ServicioClase, string queueName = "cola_clases")
        {
            _ServicioClase = ServicioClase;
            var factory = new ConnectionFactory() { HostName = _rabbitMqHostProduction };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            _queueName = queueName;
            await _channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += OnRequestReceived;

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
            Console.WriteLine($"[.] Servidor RPC Iniciado");
        }

        private async Task OnRequestReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var mensajeJson = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Recibido: {mensajeJson}");

            // Procesar la solicitud y generar la respuesta

            var mensaje = JsonSerializer.Deserialize<SolicitudRPCDTO>(mensajeJson);
            var respuesta = await procesarMensajeAsync(mensaje);

            var props = ea.BasicProperties;
            var replyProps = new BasicProperties
            {
                CorrelationId = props.CorrelationId
            };

            var respuestaJson = JsonSerializer.Serialize(respuesta);
            var responseBytes = Encoding.UTF8.GetBytes(respuestaJson);
            await _channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: props.ReplyTo,
                mandatory: false,
                basicProperties: replyProps,
                body: responseBytes
            );

            await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        }

        private async Task<RespuestaRPCDTO> procesarMensajeAsync(SolicitudRPCDTO mensaje) {
            RespuestaRPCDTO respuesta = new RespuestaRPCDTO();
            if (mensaje == null)
            {
                respuesta.Success = false;
                respuesta.Error = new ErrorDTO
                {
                    Mensaje = "No se enviaron datos"
                };

                return respuesta;
            }

            respuesta = await realizarAccionAsync(mensaje);

            return respuesta;
        }

        private async Task<RespuestaRPCDTO> realizarAccionAsync(SolicitudRPCDTO mensaje)
        {
            string accion = mensaje.Accion;
            switch (accion)
            {
                case "obtenerClasesTareasYRespuesta":
                    return await _ServicioClase.ObtenerClasesTareasRespuestasDeAlumnoAsync(mensaje.IdAlumno);
                default:
                    return new RespuestaRPCDTO
                    {
                        Success = false,
                        Error = new ErrorDTO
                        {
                            Mensaje = "Accion no soportada"
                        }
                    };
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
}   