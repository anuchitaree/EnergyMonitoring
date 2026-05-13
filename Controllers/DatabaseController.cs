using EnergyMonitoring.Interfaces;
using EnergyMonitoring.Models;
using EnergyMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace EnergyMonitoring.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {

        private readonly IDatabaseInterface _db;
        private readonly IDatapreparing _prepare;
        private readonly ILogger<DatabaseController> _logger;
        public DatabaseController(ILogger<DatabaseController> logger,
            IDatapreparing prepare,
            IDatabaseInterface database)
        {
            _logger = logger;
           _db = database;
            _prepare = prepare;
        }

        [HttpPost("post-pzemraw")]
        public async Task<ActionResult> PostPzemRaw([FromBody] List<PzemRaw> request)
        {
            var result = await _db.PostPzemRaw(request);
           var processresult =  await _prepare.MinutelyProcess(request);
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