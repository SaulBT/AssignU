using Microsoft.Extensions.Hosting;
using ServicioTareas.Config;
using System.Threading;
using System.Threading.Tasks;

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