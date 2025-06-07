using AKDashboard.API.Database;

namespace AKDashboard.API.Services
{
    public class ClimaAgendadorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ClimaAgendadorService> _logger;
        private readonly string[] _capitais = { "Curitiba", "São Paulo", "Rio de Janeiro" };

        public ClimaAgendadorService(IServiceScopeFactory scopeFactory, ILogger<ClimaAgendadorService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var climaService = scope.ServiceProvider.GetRequiredService<IClimaService>();
                    var context = scope.ServiceProvider.GetRequiredService<ClimaDbContext>();

                    foreach (var cidade in _capitais)
                    {
                        try
                        {
                            var dados = await climaService.ObterClimaAsync(cidade);
                            context.Climas.Add(dados);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Erro ao buscar clima de {cidade}: {ex.Message}");
                        }
                    }

                    await context.SaveChangesAsync();
                }

                _logger.LogInformation("Consulta automática de clima realizada.");

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}
