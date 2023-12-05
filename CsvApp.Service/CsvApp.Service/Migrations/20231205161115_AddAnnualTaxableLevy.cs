using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CsvApp.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnualTaxableLevy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AnnualTaxableLevy",
                table: "Vehicles",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualTaxableLevy",
                table: "Vehicles");
        }
    }
}
