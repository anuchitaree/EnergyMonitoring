using EnergyMonitoring.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EnergyMonitoring.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DataVisualizeController : ControllerBase
    {

        private readonly IVisualizeService visualizeService;
        private readonly ILogger<DataVisualizeController> _logger;
        public DataVisualizeController(ILogger<DataVisualizeController> logger,
            IVisualizeService visualizeService)
        {
            _logger = logger;
            this.visualizeService = visualizeService;
        }

        [HttpGet("one-day")]
        public async Task<IActionResult> GetOneDayData()
        {
            var data = await visualizeService.GetEnergyMinutesAsync(DateTime.Now);
            if(data == null || data.Count == 0)
            {
                return NotFound("No energy data found for today.");
            }
            return Ok(data);
        }











    }
}
