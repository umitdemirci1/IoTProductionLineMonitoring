using IoTProductionLineMonitoring.Context;
using IoTProductionLineMonitoring.Models;
using Microsoft.AspNetCore.SignalR;

namespace IoTProductionLineMonitoring.Services
{
    public class MockDataService : IHostedService, IDisposable
    {
        private static readonly Random Random = new();
        private Timer _timer;
        private readonly IHubContext<SensorHub> _hubContext;
        private readonly ILogger<MockDataService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MockDataService(IHubContext<SensorHub> hubContext, ILogger<MockDataService> logger, IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MockDataService is starting.");
            _timer = new Timer(GenerateAndBroadcastData, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MockDataService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private List<Sensor> GetSensors()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IoTProductionLineMonitoringContext>();
                return dbContext.Sensors.ToList();
            }
        }

        private List<SensorData> GenerateSensorData()
        {
            var sensors = GetSensors();
            var newSensorDataList = new List<SensorData>();

            foreach (var sensor in sensors)
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
                    SensorId = sensor.Id,
                    Value = value,
                    Timestamp = DateTime.Now
                };

                newSensorDataList.Add(newData);

                _logger.LogInformation($"New data generated for sensor {sensor.Id} at {newData.Timestamp}: {newData.Value}");
            }

            return newSensorDataList;
        }

        private double GenerateIncreasingValue(int sensorId)
        {
            var lastValue = 0.0;
            return lastValue + Random.NextDouble() * 5;
        }

        private void GenerateAndBroadcastData(object? state)
        {
            var newSensorData = GenerateSensorData();

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IoTProductionLineMonitoringContext>();
                dbContext.SensorData.AddRange(newSensorData);
                dbContext.SaveChanges();
            }

            _hubContext.Clients.All.SendAsync("ReceiveSensorData", newSensorData);
        }
    }
}
