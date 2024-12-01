//using IoTProductionLineMonitoring.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace IoTProductionLineMonitoring.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class SensorDataController : ControllerBase
//    {
//        private readonly MockDataService _mockDataService;

//        public SensorDataController(MockDataService mockDataService)
//        {
//            _mockDataService = mockDataService;
//        }

//        [HttpGet("{sensorId}")]
//        public IActionResult GetSensorData(int sensorId)
//        {
//            var data = _mockDataService.GetSensorData(sensorId);
//            if (!data.Any())
//                return NotFound("No data found for the given sensor.");

//            return Ok(data);
//        }
//    }
//}
