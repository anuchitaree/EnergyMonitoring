using EnergyMonitoring.Interfaces;

namespace EnergyMonitoring.Workers
{
    public class HouryWorker : BackgroundService
    {
        private readonly ILogger<HouryWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _period = TimeSpan.FromHours(1);

        public HouryWorker(ILogger<HouryWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Hourly Worker started at: {time}", DateTimeOffset.Now);

            // ใช้ PeriodicTimer แทน Timer แบบเดิมใน .NET 8
            using PeriodicTimer timer = new PeriodicTimer(_period);

            // ทำงานทันที 1 ครั้งเมื่อ Start (ถ้าต้องการ)
            using (var scope = _scopeFactory.CreateScope())
            {
                var _datapreparing = scope.ServiceProvider.GetRequiredService<IDatapreparing>();
                {
                    await _datapreparing.HourlyDataNow(DateTime.Now);
                    await _datapreparing.HourlyDataLast(DateTime.Now);
                }
            }
            // รอจนกว่าจะครบ 1 ชั่วโมง แล้วทำงานต่อใน Loop
            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                await Task.Delay(5000);
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _datapreparing = scope.ServiceProvider.GetRequiredService<IDatapreparing>();
                    {
                        await _datapreparing.HourlyDataNow(DateTime.Now);
                        await _datapreparing.HourlyDataLast(DateTime.Now);
                    }
                }

            }
        }


    }
}
