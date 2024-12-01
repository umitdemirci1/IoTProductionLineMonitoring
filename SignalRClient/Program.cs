using IoTProductionLineMonitoring.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/sensorHub")
                .Build();


            connection.On<List<SensorData>>("ReceiveSensorData", data =>
            {
                Console.Clear();
                foreach (var sensorData in data)
                {
                    Console.WriteLine($"SensorId: {sensorData.SensorId}, Value: {sensorData.Value}, Timestamp: {sensorData.Timestamp}");
                }
            });

            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR Hub. Listening for data...");

            Console.ReadLine();
        }
    }
}
