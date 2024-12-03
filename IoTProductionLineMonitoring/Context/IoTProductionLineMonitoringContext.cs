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
        public DbSet<ProductionLine> ProductionLines { get; set; }

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

            modelBuilder.Entity<ProductionLine>()
                .HasMany(pl => pl.Sensors)
                .WithOne(s => s.ProductionLine)
                .HasForeignKey(s => s.ProductionLineId);

            modelBuilder.Entity<ProductionLine>().HasData(
                new ProductionLine { Id = 1, Name = "Line1", Location = "Location1" },
                new ProductionLine { Id = 2, Name = "Line2", Location = "Location2" },
                new ProductionLine { Id = 3, Name = "Line3", Location = "Location3" }
            );

            modelBuilder.Entity<Sensor>().HasData(
                new Sensor { Id = 1, Name = "TempSensor1", Type = "Temperature", ProductionLineId = 1 },
                new Sensor { Id = 2, Name = "VibSensor1", Type = "Vibration", ProductionLineId = 1 },
                new Sensor { Id = 3, Name = "TempSensor2", Type = "Temperature", ProductionLineId = 2 },
                new Sensor { Id = 4, Name = "PowerSensor1", Type = "Power", ProductionLineId = 2 },
                new Sensor { Id = 5, Name = "EnergySensor1", Type = "Energy", ProductionLineId = 3 },
                new Sensor { Id = 6, Name = "TempSensor3", Type = "Temperature", ProductionLineId = 3 },
                new Sensor { Id = 7, Name = "VibSensor2", Type = "Vibration", ProductionLineId = 3 }
            );
        }
    }
}
