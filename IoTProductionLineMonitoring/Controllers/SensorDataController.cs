using IoTProductionLineMonitoring.Context;
using IoTProductionLineMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionLineMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly MockDataService _mockDataService;
        private readonly IoTProductionLineMonitoringContext _context;
        public SensorDataController(MockDataService mockDataService, IoTProductionLineMonitoringContext context)
        {
            _context = context;
            _mockDataService = mockDataService;
        }

        [HttpGet("SensorData")]
        public IActionResult GetSensorData(int sensorId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var data = _context.SensorData
                    .Where(sd => sd.SensorId == sensorId && sd.Timestamp >= startDate && sd.Timestamp <= endDate)
                    .OrderBy(sd => sd.Timestamp)
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching data.", error = ex.Message });
            }
        }

        [HttpGet("sensor/{sensorId}/analytics")]
        public async Task<IActionResult> GetSensorAnalytics(int sensorId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var sensor = await _context.Sensors.FindAsync(sensorId);
            if (sensor == null) return NotFound("Sensor not found.");

            var sensorData = await _context.SensorData
                .Where(data => data.SensorId == sensorId && data.Timestamp >= startDate && data.Timestamp <= endDate)
                .ToListAsync();

            if (!sensorData.Any()) return Ok("No data found for the selected range.");

            var analytics = new
            {
                Average = sensorData.Average(data => data.Value),
                MaxValue = sensorData.Max(data => data.Value),
                MinValue = sensorData.Min(data => data.Value),
                DataCount = sensorData.Count,
                GroupedByHour = sensorData
                    .GroupBy(data => data.Timestamp.Hour)
                    .Select(g => new { Hour = g.Key, Average = g.Average(d => d.Value) })
                    .OrderBy(g => g.Hour)
                    .ToList()
            };

            return Ok(analytics);
        }
    }
}
