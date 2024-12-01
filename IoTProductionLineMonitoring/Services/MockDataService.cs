using IoTProductionLineMonitoring.Models;
using Microsoft.AspNetCore.SignalR;

namespace IoTProductionLineMonitoring.Services
{
    public class MockDataService
    {
        private static readonly Random Random = new();
        private readonly List<SensorData> _sensorDataList = new();
        private readonly Timer _timer;
        private readonly IHubContext<SensorHub> _hubContext;
        private readonly ILogger<MockDataService> _logger;

        public MockDataService(IHubContext<SensorHub> hubContext, ILogger<MockDataService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
            Sensors = new List<Sensor>
                {
                    new Sensor { Id = 1, Name = "TempSensor1", Type = "Temperature" },
                    new Sensor { Id = 2, Name = "VibSensor1", Type = "Vibration" },
                    new Sensor { Id = 3, Name = "TempSensor2", Type = "Temperature" },
                    new Sensor { Id = 4, Name = "PowerSensor1", Type = "Power" },
                    new Sensor { Id = 5, Name = "EnergySensor1", Type = "Energy" }
                };

            _timer = new Timer(GenerateAndBroadcastData, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public List<Sensor> Sensors { get; }

        public List<Sensor> GetSensors()
        {
            return Sensors;
        }

        public IEnumerable<SensorData> GetSensorData(int sensorId)
        {
            return _sensorDataList.Where(data => data.SensorId == sensorId);
        }

        private void GenerateSensorData(object? state)
        {
            foreach (var sensor in Sensors)
            {
                double value = sensor.Type switch
                {
                    "Temperature" => Random.NextDouble() * 10 + 20,
                    "Energy" => GenerateIncreasingValue(sensor.Id),
                    "Vibration" => Random.NextDouble() * 10,
                    _ => Random.NextDouble() * 100
                };

                var newData = new SensorData
                {
                    Id = _sensorDataList.Count + 1,
                    SensorId = sensor.Id,
                    Value = value,
                    Timestamp = DateTime.Now
                };

                _sensorDataList.Add(newData);
                _logger.LogInformation($"New data generated for sensor {sensor.Id} at {newData.Timestamp}: {newData.Value}");
            }
        }

        private double GenerateIncreasingValue(int sensorId)
        {
            var lastValue = _sensorDataList.LastOrDefault(d => d.SensorId == sensorId)?.Value ?? 0;
            return lastValue + Random.NextDouble() * 5;
        }

        private void GenerateAndBroadcastData(object? state)
        {
            GenerateSensorData(null);
            _hubContext.Clients.All.SendAsync("ReceiveSensorData", _sensorDataList);
        }
    }
}
