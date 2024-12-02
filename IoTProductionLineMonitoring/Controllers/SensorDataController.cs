using IoTProductionLineMonitoring.Context;
using IoTProductionLineMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
