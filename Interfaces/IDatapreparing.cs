namespace EnergyMonitoring.Interfaces
{
    public interface IDatapreparing
    {
        Task<bool> DailyDataToday(DateTime nowDateTime);
        Task<bool> DailyDataYesterday(DateTime nowDateTime);
        Task<bool> HourlyDataNow(DateTime nowDateTime);
        Task<bool> HourlyDataLast(DateTime nowDateTime);
        Task<bool> FifteenMinuteData(DateTime nowDateTime);
    }
}
