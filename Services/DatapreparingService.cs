using EnergyMonitoring.Data;
using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using EnergyMonitoring.Modules;
using Microsoft.EntityFrameworkCore;

namespace EnergyMonitoring.Services
{
    public class DatapreparingService : IDatapreparing
    {
        private readonly ILogger<DatapreparingService> _logger;
        private readonly IDatabaseInterface _dbService;
        private readonly EnergyContext db;
        public DatapreparingService(EnergyContext databaseContext,
            ILogger<DatapreparingService> logger,
            IDatabaseInterface dbService)
        {
            _logger = logger;
            _dbService = dbService;
            db = databaseContext;
        }

        public async Task<bool> DailyDataYesterday(DateTime nowDateTime)
        {
            try
            {
                DateTime lastDay = nowDateTime.AddDays(-1);
                var date = DateTimeFunc.DataInOnedayUtc(lastDay);

                var daysKey = DateTimeFunc.SetDayUtc(lastDay);
                var lasthourTable = await db.EnergyHours
                                        .Where(d => d.Hour >= date.FromDateTime && d.Hour <= date.ToDateTime).ToListAsync();
                if (lasthourTable.Count == 0)
                {
                    _logger.LogInformation("No data found for the last day: {day}", daysKey);
                    return false;
                }

                var sumEnergy = lasthourTable.Sum(d => d.EnergyKwh);
                var maxPower = lasthourTable.Max(d => d.MaxPower);


                var checklastupdate = await db.EnergyDays
                        .Where(d => d.Day == daysKey)
                        .FirstOrDefaultAsync();

                if (checklastupdate == null)
                {
                    var newDayData = new EnergyDay
                    {
                        Day = daysKey,
                        EnergyKwh = sumEnergy,
                        MaxPower = maxPower
                    };
                    db.EnergyDays.Add(newDayData);
                    await db.SaveChangesAsync();
                }
                else
                {
                    checklastupdate.EnergyKwh = sumEnergy;
                    checklastupdate.MaxPower = maxPower;
                    db.EnergyDays.Update(checklastupdate);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> FifteenMinuteData(DateTime nowDateTime)
        {
            try
            {
                DateTime lastDay = nowDateTime.AddDays(0);
                var (daysKey, date) = DateTimeFunc.Data15Minute(DateTimeFunc.FindQ15Minute(lastDay), lastDay);

                var lasthourTable = await db.EnergyMinutes
                                        .Where(d => d.Minute >= date.FromDateTime &&
                                        d.Minute <= date.ToDateTime).ToListAsync();
                if (lasthourTable.Count == 0)
                {
                    _logger.LogInformation("No data found for the last day: {day}", daysKey);
                    return false;
                }

                var sumEnergy = lasthourTable.Sum(d => d.EnergyKwh);
                var maxPower = lasthourTable.Max(d => d.MaxPower);


                var checklastupdate = await db.Energy15Minutes
                        .Where(d => d.Minute == daysKey)
                        .FirstOrDefaultAsync();

                if (checklastupdate == null)
                {
                    var newDayData = new Energy15Minute
                    {
                        Minute = daysKey,
                        EnergyKwh = sumEnergy,
                        MaxPower = maxPower
                    };
                    db.Energy15Minutes.Add(newDayData);
                    await db.SaveChangesAsync();
                }
                else
                {
                    checklastupdate.EnergyKwh = sumEnergy;
                    checklastupdate.MaxPower = maxPower;
                    db.Energy15Minutes.Update(checklastupdate);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> DailyDataToday(DateTime nowDateTime)
        {
            try
            {
                DateTime lastDay = nowDateTime.AddDays(0);
                var date = DateTimeFunc.DataInOnedayUtc(lastDay);

                var daysKey = DateTimeFunc.SetDayUtc(lastDay);
                var lasthourTable = await db.EnergyHours
                                        .Where(d => d.Hour >= date.FromDateTime && d.Hour <= date.ToDateTime).ToListAsync();
                if (lasthourTable.Count == 0)
                {
                    _logger.LogInformation("No data found for the last day: {day}", daysKey);
                    return false;
                }

                var sumEnergy = lasthourTable.Sum(d => d.EnergyKwh);
                var maxPower = lasthourTable.Max(d => d.MaxPower);


                var checklastupdate = await db.EnergyDays
                        .Where(d => d.Day == daysKey)
                        .FirstOrDefaultAsync();

                if (checklastupdate == null)
                {
                    var newDayData = new EnergyDay
                    {
                        Day = daysKey,
                        EnergyKwh = sumEnergy,
                        MaxPower = maxPower
                    };
                    db.EnergyDays.Add(newDayData);
                    await db.SaveChangesAsync();
                }
                else
                {
                    checklastupdate.EnergyKwh = sumEnergy;
                    checklastupdate.MaxPower = maxPower;
                    db.EnergyDays.Update(checklastupdate);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> HourlyDataNow(DateTime nowDateTime)
        {
            try
            {
                DateTime lastDay = nowDateTime.AddDays(0);
                var date = DateTimeFunc.DataInOneHourUtc(lastDay);

                var daysKey = DateTimeFunc.SetHourUtc(lastDay);
                var lastMinuteTable = await db.EnergyMinutes
                                        .Where(d => d.Minute >= date.FromDateTime && d.Minute <= date.ToDateTime).ToListAsync();
                if (lastMinuteTable.Count == 0)
                {
                    _logger.LogInformation("No data found for the last hour: {hour}", daysKey);
                    return false;
                }

                var sumEnergy = lastMinuteTable.Sum(d => d.EnergyKwh);
                var maxPower = lastMinuteTable.Max(d => d.MaxPower);


                var checklastupdate = await db.EnergyHours
                        .Where(d => d.Hour == daysKey)
                        .FirstOrDefaultAsync();

                if (checklastupdate == null)
                {
                    var newDayData = new EnergyHour
                    {
                        Hour = daysKey,
                        EnergyKwh = sumEnergy,
                        MaxPower = maxPower
                    };
                    db.EnergyHours.Add(newDayData);
                    await db.SaveChangesAsync();
                }
                else
                {
                    checklastupdate.EnergyKwh = sumEnergy;
                    checklastupdate.MaxPower = maxPower;
                    db.EnergyHours.Update(checklastupdate);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }



        }

        public async Task<bool> HourlyDataLast(DateTime nowDateTime)
        {
            try
            {
                DateTime lastDay = nowDateTime.AddDays(-1);
                var date = DateTimeFunc.DataInOneHourUtc(lastDay);

                var daysKey = DateTimeFunc.SetHourUtc(lastDay);
                var lastMinuteTable = await db.EnergyMinutes
                                        .Where(d => d.Minute >= date.FromDateTime && d.Minute <= date.ToDateTime).ToListAsync();
                if (lastMinuteTable.Count == 0)
                {
                    _logger.LogInformation("No data found for the last hour: {hour}", daysKey);
                    return false;
                }

                var sumEnergy = lastMinuteTable.Sum(d => d.EnergyKwh);
                var maxPower = lastMinuteTable.Max(d => d.MaxPower);


                var checklastupdate = await db.EnergyHours
                        .Where(d => d.Hour == daysKey)
                        .FirstOrDefaultAsync();

                if (checklastupdate == null)
                {
                    var newDayData = new EnergyHour
                    {
                        Hour = daysKey,
                        EnergyKwh = sumEnergy,
                        MaxPower = maxPower
                    };
                    db.EnergyHours.Add(newDayData);
                    await db.SaveChangesAsync();
                }
                else
                {
                    checklastupdate.EnergyKwh = sumEnergy;
                    checklastupdate.MaxPower = maxPower;
                    db.EnergyHours.Update(checklastupdate);
                    await db.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
