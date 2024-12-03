namespace IoTProductionLineMonitoring.Models
{
    public class ProductionLine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public ICollection<Sensor> Sensors { get; set; }
    }
}
