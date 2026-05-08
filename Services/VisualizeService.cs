using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;

namespace EnergyMonitoring.Services
{
    public class VisualizeService : IVisualizeService
    {
        private readonly IDatabaseInterface db;
        private readonly ILogger<VisualizeService> _logger;
        public VisualizeService(IDatabaseInterface databaseInterface,
            ILogger<VisualizeService> logger)
        {
                db = databaseInterface;
                _logger = logger;
        }
        public async Task<List<EnergyMinute>> GetEnergyMinutesAsync(DateTime date)
        {
            var energyMinutes =await db.GetEnergyMinuteOneday(date);
            return energyMinutes;
        }
    }
}
