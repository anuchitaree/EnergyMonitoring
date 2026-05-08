using EnergyMonitoring.Models;

namespace EnergyMonitoring.Interfaces
{
    public interface IVisualizeService
    {

        Task<List<EnergyMinute>> GetEnergyMinutesAsync(DateTime date);

       

    }
}
