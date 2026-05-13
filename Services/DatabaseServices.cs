using EnergyMonitoring.Data;
using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using EnergyMonitoring.Modules;
using Microsoft.EntityFrameworkCore;

namespace EnergyMonitoring.Services
{
    public class DatabaseServices : IDatabaseInterface
    {

        private readonly EnergyContext db;
        private readonly ILogger<DatabaseServices> _logger;

        public DatabaseServices(EnergyContext databaseContext,
            ILogger<DatabaseServices> logger)
        {
            db = databaseContext;
            _logger = logger;
        }



        




        public Task<List<Energy15Minute>> GetEnergy15Minute(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<List<EnergyDay>> GetEnergyDay(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<List<EnergyHour>> GetEnergyHour(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<List<EnergyMinute>> GetEnergyMinute(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EnergyMinute>> GetEnergyMinuteOneday(DateTime now)
        {
            var fromto = DateTimeFunc.DataInOnedayUtc(now);

            var result = await db.EnergyMinutes.
                        Where(x => x.Minute >= fromto.FromDateTime && x.Minute <= fromto.ToDateTime)
                        .Select(x => new EnergyMinute
                        {
                            Id = x.Id,
                            Minute = x.Minute.ToLocalTime(),
                            EnergyKwh = x.EnergyKwh,
                            MaxPower = x.MaxPower
                        })
                        .ToListAsync();
            return result;
        }

        public Task<List<PzemRaw>> GetPzemRaw()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostEnergy15Minute(Energy15Minute raw)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostEnergyDay(EnergyDay raw)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostEnergyHour(EnergyHour raw)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> PostEnergyMinute(EnergyMinute raw)
        {
            try
            {
                var check = db.EnergyMinutes.
               Where(x => x.Minute == raw.Minute).FirstOrDefault();
                if (check == null)
                {
                    db.EnergyMinutes.Add(raw);
                }
                else
                {
                    check.EnergyKwh = raw.EnergyKwh;
                    check.MaxPower = raw.MaxPower;
                    db.EnergyMinutes.Update(check);
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> PostPzemRaw(PzemRaw raw)
        {
            try
            {
                await db.PzemRaws.AddAsync(raw);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> PutEnergy15Minute(Energy15Minute raw)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutEnergyDay(EnergyDay raw)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutEnergyHour(EnergyHour raw)
        {
            throw new NotImplementedException();
        }

<<<<<<< HEAD
        public  async Task<bool> PutEnergyMinute(EnergyMinute raw)
=======
        public async Task<bool> PutEnergyMinute(EnergyMinute raw)
>>>>>>> 89b4663897b39168111e4244143aa9d7782a03e6
        {
            try
            {
                var check = db.EnergyMinutes.
               Where(x => x.Minute == raw.Minute).FirstOrDefault();
                if (check == null)
                {
                    db.EnergyMinutes.Add(raw);
                }
                else
                {
                    check.EnergyKwh = raw.EnergyKwh;
                    check.MaxPower = raw.MaxPower;
                    db.EnergyMinutes.Update(check);
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    
}
