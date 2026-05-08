using EnergyMonitoring.Interfaces;



namespace EnergyMonitoring.Workers
{
    public class DailyWorker : BackgroundService
    {
        private readonly ILogger<DailyWorker> _logger;
        private readonly TimeSpan _period = TimeSpan.FromDays(1);
        private readonly IServiceScopeFactory _scopeFactory;
        public DailyWorker(ILogger<DailyWorker> logger,
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
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
                    await _datapreparing.DailyDataYesterday(DateTime.Now);
                    await _datapreparing.DailyDataToday(DateTime.Now);
                }
            }


            // รอจนกว่าจะครบ 1 ชั่วโมง แล้วทำงานต่อใน Loop
            while (!stoppingToken.IsCancellationRequested &&
                   await timer.WaitForNextTickAsync(stoppingToken))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _datapreparing = scope.ServiceProvider.GetRequiredService<IDatapreparing>();
                    {
                        await _datapreparing.DailyDataYesterday(DateTime.Now);
                        await _datapreparing.DailyDataToday(DateTime.Now);
                    }
                }
            }
        }



    }
}