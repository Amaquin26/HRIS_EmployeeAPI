using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRIS_Employee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShiftRecordMinorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFlag",
                table: "ShiftRecords",
                newName: "IsFlagged");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ClockOut",
                table: "ShiftRecords",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFlagged",
                table: "ShiftRecords",
                newName: "IsFlag");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ClockOut",
                table: "ShiftRecords",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
