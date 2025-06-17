using ServicioClases.Config;
using ServicioClases.Services.Interfaces;

namespace ServicioUsuarios.BackgroundServices
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
            var servicioAlumno = scope.ServiceProvider.GetRequiredService<IServicioClase>();

            await rpcServer.InicializarServidor(servicioAlumno);
            await Task.Delay(Timeout.Infinite, stoppingToken); // Mantener el servicio en ejecución
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}