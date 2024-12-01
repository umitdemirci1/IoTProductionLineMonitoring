using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTProductionLineMonitoring.Migrations
{
    /// <inheritdoc />
    public partial class DataSeedSensors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "TempSensor1", "Temperature" },
                    { 2, "VibSensor1", "Vibration" },
                    { 3, "TempSensor2", "Temperature" },
                    { 4, "PowerSensor1", "Power" },
                    { 5, "EnergySensor1", "Energy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
