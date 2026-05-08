using EnergyMonitoring.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace EnergyMonitoring.Data
{
    public class EnergyContext : DbContext
    {

        public DbSet<PzemRaw> PzemRaws { get; set; }
        public DbSet<EnergyDay> EnergyDays { get; set; }
        public DbSet<EnergyHour> EnergyHours { get; set; }
        public DbSet<EnergyMinute> EnergyMinutes { get; set; }

        public DbSet<Energy15Minute> Energy15Minutes { get; set; }



        public EnergyContext(DbContextOptions<EnergyContext> options)
                  : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
    }
}
