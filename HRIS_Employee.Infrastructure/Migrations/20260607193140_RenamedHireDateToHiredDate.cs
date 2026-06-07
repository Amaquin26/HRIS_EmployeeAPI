using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS_Employee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedHireDateToHiredDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HireDate",
                table: "Employees",
                newName: "HiredDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HiredDate",
                table: "Employees",
                newName: "HireDate");
        }
    }
}
