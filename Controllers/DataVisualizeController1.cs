using EnergyMonitoring.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EnergyMonitoring.Controllers
{
    [ApiController]
    [Route("api/v1/data-visualize")]
    public class DataVisualizeController1 : ControllerBase
    {

        private readonly IVisualizeService visualizeService;
        private readonly ILogger<DataVisualizeController1> _logger;
        public DataVisualizeController1(ILogger<DataVisualizeController1> logger,
            IVisualizeService visualizeService)
        {
            _logger = logger;
            this.visualizeService = visualizeService;
        }

        [HttpGet("one-day")]
        public async Task<IActionResult> GetOneDayData()
        {
            var data = await visualizeService.GetEnergyMinutesAsync(DateTime.Now);
            if (data == null || data.Count == 0)
            {
                return NotFound("No energy data found for today.");
            }
            return Ok(data);
        }











    }
}