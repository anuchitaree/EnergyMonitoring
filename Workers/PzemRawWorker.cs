using EnergyMonitoring.Data;
using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using EnergyMonitoring.Services;
using Microsoft.Extensions.Options;
using System.Diagnostics.Metrics;
using System.Runtime;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EnergyMonitoring.Workers
{
    public class PzemRawWorker : BackgroundService
    {
        private readonly ILogger<PzemRawWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly ModbusSetting _mobusSetting;
        private readonly ModbusService _modbus ;

        private float? _lastEnergy = null;

        private readonly Dictionary<DateTime, float> _minuteBuffer = new();

        private readonly List<float> _energerTotal = new();
        private readonly List<float> _powerTotal = new();

        private DateTime _currentMinute;

        public PzemRawWorker(ILogger<PzemRawWorker> logger,
            IServiceScopeFactory scopeFactory,
            IOptions<ModbusSetting> mobusSetting)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _mobusSetting = mobusSetting.Value;
            try
            {
                _modbus = new ModbusService(
               _mobusSetting.Port,
               _mobusSetting.BaudRate,
               _mobusSetting.DataBits,
               _mobusSetting.StopBits,
               _mobusSetting.Parity,
               _mobusSetting.SlaveId
           );
                _currentMinute = GetMinuteKey(DateTime.Now);
            }
            catch 
            {
                // fallback ป้องกัน warning
               _modbus = new ModbusService("COM1", 9600, 8, 1, "None", 1);
               _currentMinute = GetMinuteKey(DateTime.Now);
            }

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nowUtc = DateTime.UtcNow;
                var minuteKey = GetMinuteKey(DateTime.Now);


                try
                {
                    var data = _modbus.ReadData(); // ต้องมี EnergyTotal

                    float delta = 0;

                    if (_lastEnergy.HasValue)
                    {
                        delta = data.Energy - _lastEnergy.Value;

                        // กัน reset / overflow
                        if (delta < 0 || delta > 5)
                        {
                            _logger.LogWarning("Energy reset detected");
                            delta = 0;
                        }
                    }
                    _lastEnergy = data.Energy;

                    // 👉 เก็บ RAW
                    _logger.LogInformation($" *****************  V={data.Voltage},P={data.Power},E={data.Energy} *******************");
                    //await SaveRaw(data, nowUtc);  // every 5 วินาที




                    // 👉 สะสมรายนาที
                    if (!_minuteBuffer.ContainsKey(minuteKey))
                    {
                        _minuteBuffer[minuteKey] = 0;

                    }

                    _energerTotal.Add(delta);
                    _powerTotal.Add(data.Power);

                    _minuteBuffer[minuteKey] += delta;

                    // 👉 ถ้านาทีเปลี่ยน → flush
                    if (minuteKey != _currentMinute)
                    {

                        var totalEnergy = _energerTotal.Sum();
                        var PeakPower = _powerTotal.Count > 0 ? _powerTotal.Max() : 0;

                        //await FlushMinute(_currentMinute, totalEnergy, PeakPower);

                        _energerTotal.Clear();
                        _powerTotal.Clear();
                        _minuteBuffer.Remove(_currentMinute);
                        _currentMinute = minuteKey;
                    }


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reading Modbus");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }




        private DateTime GetMinuteKey(DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        }

        // 🔹 save raw
        //private async Task SaveRaw(dynamic data, DateTime now)
        //{
        //    try
        //    {
                //using (var scope = _scopeFactory.CreateScope())
                //{
                //    var repo = scope.ServiceProvider.GetRequiredService<IDatabaseInterface>();

                //    var result = new PzemRaw
                //    {
                //        Timestamp = now,
                //        Voltage = data.Voltage,
                //        Current = data.Current,
                //        Power = data.Power,
                //        Energy = data.Energy,
                //        Frequency = data.Frequency,
                //        PowerFactor = data.PowerFactor,
                //        Alarm = data.Alarm,
                //    };

                //    await repo.PostPzemRaw(result);
                //}
        //    }
        //    catch 
        //    {

        //    }
           

           
           
        //}

        // 🔹 flush นาที
        private async Task FlushMinute(DateTime minute, float totalEnergy, float peakPower)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IDatabaseInterface>();

                    var data = new EnergyMinute
                    {
                        Minute = minute.ToUniversalTime(),
                        EnergyKwh = totalEnergy,
                        MaxPower = peakPower
                    };

                    var result = await repo.PostEnergyMinute(data);
                    if (!result)
                    {
                        _logger.LogError("Failed to save EnergyMinute data");
                    }
                    else
                    {
                        _logger.LogInformation($"Saved minute {minute}: {totalEnergy} kWh : {peakPower} kW");
                    }
                }
            }
            catch 
            {

               
            }
            

            
          
        }
    }
}
