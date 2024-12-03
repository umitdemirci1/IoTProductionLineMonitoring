using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IoTProductionLineMonitoring.Migrations
{
    /// <inheritdoc />
    public partial class ProductionLineAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionLineId",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductionLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionLines", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProductionLines",
                columns: new[] { "Id", "Location", "Name" },
                values: new object[,]
                {
                    { 1, "Location1", "Line1" },
                    { 2, "Location2", "Line2" },
                    { 3, "Location3", "Line3" }
                });

            migrationBuilder.UpdateData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProductionLineId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProductionLineId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProductionLineId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProductionLineId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProductionLineId",
                value: 3);

            migrationBuilder.InsertData(
                table: "Sensors",
                columns: new[] { "Id", "Name", "ProductionLineId", "Type" },
                values: new object[,]
                {
                    { 6, "TempSensor3", 3, "Temperature" },
                    { 7, "VibSensor2", 3, "Vibration" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ProductionLineId",
                table: "Sensors",
                column: "ProductionLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_ProductionLines_ProductionLineId",
                table: "Sensors",
                column: "ProductionLineId",
                principalTable: "ProductionLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_ProductionLines_ProductionLineId",
                table: "Sensors");

            migrationBuilder.DropTable(
                name: "ProductionLines");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_ProductionLineId",
                table: "Sensors");

            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sensors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "ProductionLineId",
                table: "Sensors");
        }
    }
}
