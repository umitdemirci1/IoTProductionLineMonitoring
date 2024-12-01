using IoTProductionLineMonitoring.Models;
using Microsoft.EntityFrameworkCore;

namespace IoTProductionLineMonitoring.Context
{
    public class IoTProductionLineMonitoringContext : DbContext
    {
        public IoTProductionLineMonitoringContext(DbContextOptions<IoTProductionLineMonitoringContext> options)
            : base(options)
        {
        }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<SensorData> SensorData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Sensor>()
                .HasMany(s => s.SensorData)
                .WithOne(sd => sd.Sensor)
                .HasForeignKey(sd => sd.SensorId);

            modelBuilder.Entity<SensorData>()
                .Property(sd => sd.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Sensor>().HasData(
                new Sensor { Id = 1, Name = "TempSensor1", Type = "Temperature" },
                new Sensor { Id = 2, Name = "VibSensor1", Type = "Vibration" },
                new Sensor { Id = 3, Name = "TempSensor2", Type = "Temperature" },
                new Sensor { Id = 4, Name = "PowerSensor1", Type = "Power" },
                new Sensor { Id = 5, Name = "EnergySensor1", Type = "Energy" }
            );
        }
    }
}
