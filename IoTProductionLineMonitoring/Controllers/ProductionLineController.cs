using IoTProductionLineMonitoring.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionLineMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionLineController : ControllerBase
    {

        private readonly IoTProductionLineMonitoringContext _context;

        public ProductionLineController(IoTProductionLineMonitoringContext context)
        {
            _context = context;
        }

        [HttpGet("production-lines")]
        public async Task<IActionResult> GetProductionLines()
        {
            var productionLines = await _context.ProductionLines
                .Include(pl => pl.Sensors)
                .ToListAsync();

            return Ok(productionLines);
        }

        [HttpGet("production-line/{productionLineId}/analytics")]
        public async Task<IActionResult> GetProductionLineAnalytics(int productionLineId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var productionLine = await _context.ProductionLines
                .Include(pl => pl.Sensors)
                .ThenInclude(sensor => sensor.SensorData)
                .FirstOrDefaultAsync(pl => pl.Id == productionLineId);

            if (productionLine == null) return NotFound("Production line not found.");

            var analytics = productionLine.Sensors.Select(sensor => new
            {
                SensorId = sensor.Id,
                SensorName = sensor.Name,
                Average = sensor.SensorData
                  .Where(data => data.Timestamp >= startDate && data.Timestamp <= endDate)
                  .DefaultIfEmpty()
                  .Average(data => data == null ? 0 : data.Value)
            });

            return Ok(analytics);
        }
    }
}
