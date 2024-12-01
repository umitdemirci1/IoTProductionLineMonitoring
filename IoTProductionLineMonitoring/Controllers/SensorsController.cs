using IoTProductionLineMonitoring.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IoTProductionLineMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase
    {
        private readonly MockDataService _mockDataService;

        public SensorsController(MockDataService mockDataService)
        {
            _mockDataService = mockDataService;
        }

        [HttpGet]
        public IActionResult GetSensors()
        {
            var sensors = _mockDataService.GetSensors();
            return Ok(sensors);
        }
    }
}
