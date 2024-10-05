using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarsAzureExam.Migrations
{
    /// <inheritdoc />
    public partial class RenameMakeToBrand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Make",
                table: "Cars",
                newName: "Brand");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Brand",
                table: "Cars",
                newName: "Make");
        }
    }
}
