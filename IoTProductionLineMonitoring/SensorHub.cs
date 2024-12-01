using IoTProductionLineMonitoring.Models;
using Microsoft.AspNetCore.SignalR;

namespace IoTProductionLineMonitoring
{
    public class SensorHub : Hub
    {
        public async Task SendSensorData(SensorData data)
        {
            await Clients.All.SendAsync("ReceiveSensorData", data);
        }
    }
}
