using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnergyMonitoring.Controllers
{
    [Route("api/v1[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {

        private readonly IDatabaseInterface db;

        private readonly ILogger<DatabaseController> _logger;
        public DatabaseController(ILogger<DatabaseController> logger,
            IDatabaseInterface database)
        {
            _logger = logger;
            this.db = database;
        }

        //[HttpDelete("delete-pzemRaws")]
        //public async Task<IActionResult> DeletePzemRaws()
        //{
        //    return Ok("Deleted successfully");
        //}









    }
}
