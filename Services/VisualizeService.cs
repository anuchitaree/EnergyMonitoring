using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using System.IO;
using System.Text.Json;

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
           
            string path = @"c:\project\mockup\minutedata.json";
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            using var stream = File.OpenRead(path);

            var data = await JsonSerializer.DeserializeAsync<List<EnergyMinute>>(stream);
            return data ?? new List<EnergyMinute>();

            //return JsonSerializer.Deserialize<List<EnergyMinute>>(json, options);
        }
    }
}
