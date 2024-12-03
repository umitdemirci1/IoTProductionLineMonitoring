namespace IoTProductionLineMonitoring.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ProductionLineId { get; set; }

        public ICollection<SensorData> SensorData { get; set; }
        public ProductionLine ProductionLine { get; set; }
    }
}
