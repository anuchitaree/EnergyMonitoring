using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using EnergyMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EnergyMonitoring.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {

        private readonly IDatabaseInterface _db;

        private readonly ILogger<DatabaseController> _logger;
        public DatabaseController(ILogger<DatabaseController> logger,
            IDatabaseInterface database)
        {
            _logger = logger;
            _db = database;
        }

        [HttpPost("post-pzemraw")]
        public async Task<ActionResult> PostPzemRaw([FromBody] PzemRaw request)
        {
            var result = await _db.PostPzemRaw(request);
            if (result == false)
                return BadRequest();
            return Ok();
        }

        [HttpPut("put-energyminute")]
        public async Task<ActionResult> PutEnergyMinute([FromBody] EnergyMinute request)
        {
            var result = await _db.PutEnergyMinute(request);
            if (result == false)
                return BadRequest();
            return Ok();
        }









    }
}