using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvApp.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddRoadworthyTestInterval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoadworthyTestInterval",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoadworthyTestInterval",
                table: "Vehicles");
        }
    }
}
