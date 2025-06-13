using ServicioUsuarios.Config;

namespace ServicioUsuarios.BackgroundServices;

public class RabbitMqInitializer : IHostedService
{
    private readonly RpcClientRabbitMQ _rpcClient;

    public RabbitMqInitializer(RpcClientRabbitMQ rpcClient)
    {
        _rpcClient = rpcClient;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _rpcClient.InicializarCliente();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}