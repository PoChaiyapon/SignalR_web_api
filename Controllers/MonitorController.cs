using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Hubs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IHubContext<MonitorHub> _hubContext;

        public MonitorController(IDataService dataService, IHubContext<MonitorHub> hubContext)
        {
            _dataService = dataService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MonitorData>>> GetLatestData()
        {
            var data = await _dataService.GetLatestDataAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<MonitorData>> PostData(MonitorData data)
        {
            var result = await _dataService.AddDataPointAsync(data);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", result);
            return CreatedAtAction(nameof(GetLatestData), new { id = result.Timestamp }, result);
        }

        [HttpPost("manual")]
        public async Task<ActionResult<MonitorData>> TriggerManualUpdate([FromBody] MonitorData data)
        {
            if (data == null)
            {
                data = new MonitorData
                {
                    Timestamp = System.DateTime.Now,
                    MetricName = "Manual",
                    Value = new System.Random().Next(1, 100),
                    Status = "Manual Update"
                };
            }
            else
            {
                data.Timestamp = System.DateTime.Now;
            }

            var result = await _dataService.AddDataPointAsync(data);
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", result);
            return Ok(result);
        }
    }
}