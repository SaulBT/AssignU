using Microsoft.Extensions.Hosting;
using ServicioTareas.Config;
using ServicioTareas.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace ServicioTareas.BackgroundServices
{
    public class InicializadorRpcServer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public InicializadorRpcServer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var rpcServer = scope.ServiceProvider.GetRequiredService<RpcServerRabbitMQ>();
            var servicioTarea = scope.ServiceProvider.GetRequiredService<IServicioTarea>();

            await rpcServer.InicializarServidor(servicioTarea);
            await Task.Delay(Timeout.Infinite, stoppingToken); // Mantener el servicio en ejecución
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}