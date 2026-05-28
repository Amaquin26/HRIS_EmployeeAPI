using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS_Employee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceEODOffsetToCrossesMidnight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDayOffset",
                table: "ScheduleDays");

            migrationBuilder.AddColumn<bool>(
                name: "CrossesMidnight",
                table: "ScheduleDays",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrossesMidnight",
                table: "ScheduleDays");

            migrationBuilder.AddColumn<int>(
                name: "EndDayOffset",
                table: "ScheduleDays",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
