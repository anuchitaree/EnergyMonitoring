
using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;

namespace EnergyMonitoring.Workers
{
    public class MinutelyWorker : BackgroundService
    {
       
        private readonly ILogger<MinutelyWorker> _logger;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(1);
        private readonly IServiceScopeFactory _scopeFactory;

        public MinutelyWorker(ILogger<MinutelyWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hourly Worker started at: {time}", DateTimeOffset.Now);

            using PeriodicTimer timer = new PeriodicTimer(_period);

            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IDatapreparing>();

                    await repo.FifteenMinuteData(DateTime.Now);
                    await repo.HourlyDataNow(DateTime.Now);
                }
            }
        }
    }
}
