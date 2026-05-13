using EnergyMonitoring.Models;

namespace EnergyMonitoring.Interfaces
{
    public interface IDatabaseInterface
    {
        Task<List<PzemRaw>> GetPzemRaw();
        Task<bool> PostPzemRaw(List<PzemRaw> raw);


        Task<List<EnergyMinute>> GetEnergyMinute(DateTime from, DateTime to);
        Task<List<EnergyMinute>> GetEnergyMinuteOneday(DateTime now);
        Task<bool> PostEnergyMinute(EnergyMinute raw);
        Task<bool> PutEnergyMinute(EnergyMinute raw);



        Task<List<Energy15Minute>> GetEnergy15Minute(DateTime from, DateTime to);
        Task<bool> PostEnergy15Minute(Energy15Minute raw);
        Task<bool> PutEnergy15Minute(Energy15Minute raw);




        Task<List<EnergyHour>> GetEnergyHour(DateTime from, DateTime to);
        Task<bool> PostEnergyHour(EnergyHour raw);
        Task<bool> PutEnergyHour(EnergyHour raw);



        Task<List<EnergyDay>> GetEnergyDay(DateTime from, DateTime to);
        Task<bool> PostEnergyDay(EnergyDay raw);
        Task<bool> PutEnergyDay(EnergyDay raw);



    }
}
