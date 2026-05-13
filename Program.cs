using EnergyMonitoring.Data;
using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using EnergyMonitoring.Services;
using EnergyMonitoring.Workers;
using Microsoft.EntityFrameworkCore;

namespace EnergyMonitoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            ConfigurationManager configuration = builder.Configuration;

            var hostSettings = new HostSettingServices();
            builder.Configuration.Bind("HostSettingServices", hostSettings);
            builder.Services.AddSingleton(hostSettings);

            builder.Services.Configure<ModbusSetting>(
            builder.Configuration.GetSection("ModbusSettings"));


            //PostgreSQL StockContext
            builder.Services.AddDbContext<EnergyContext>(option =>
            option.UseNpgsql(builder.Configuration["PostgreConnectionStrings:DefaultConnection"], builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }));
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddTransient<IVisualizeService, VisualizeService>();
            builder.Services.AddTransient<IDatabaseInterface, DatabaseServices>();
            builder.Services.AddTransient<IDatapreparing, DatapreparingService>();

            //builder.Services.AddHostedService<PzemRawWorker>();
            builder.Services.AddHostedService<HouryWorker>();
            builder.Services.AddHostedService<DailyWorker>();
            builder.Services.AddHostedService<MinutelyWorker>();
      

           





            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();




            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<EnergyContext>();

                    logger.LogInformation("Checking database...");

                    db.Database.Migrate(); // ?? ????? DB + Table ???????????

                    logger.LogInformation("Database ready");
                }
                catch (Exception ex)
                {
                    logger.LogCritical(ex, "Database init failed");

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("? DATABASE ERROR");
                    Console.ResetColor();
                }
            }


            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}