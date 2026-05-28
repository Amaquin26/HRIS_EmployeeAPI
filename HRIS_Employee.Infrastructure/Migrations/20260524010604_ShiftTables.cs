using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HRIS_Employee.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ShiftTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Employees",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Employees",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BreakMinutes = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShiftRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmployeeId = table.Column<int>(type: "integer", nullable: false),
                    ShiftDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ClockIn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ClockOut = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsFlag = table.Column<bool>(type: "boolean", nullable: false),
                    FlagReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftRecords", x => x.Id);
                    table.CheckConstraint("CK_ShiftRecord_Status", "\"Status\" BETWEEN 0 AND 3");
                    table.ForeignKey(
                        name: "FK_ShiftRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDayOffset = table.Column<int>(type: "integer", nullable: false),
                    IsRestDay = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDays", x => x.Id);
                    table.CheckConstraint("CK_ScheduleDay_DayOfWeek", "\"DayOfWeek\" BETWEEN 0 AND 6");
                    table.ForeignKey(
                        name: "FK_ScheduleDays_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleOverrides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    SpecificDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDayOffset = table.Column<int>(type: "integer", nullable: false),
                    IsRestDay = table.Column<bool>(type: "boolean", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleOverrides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduleOverrides_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShiftAdjustments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShiftId = table.Column<int>(type: "integer", nullable: false),
                    AdjustedById = table.Column<int>(type: "integer", nullable: false),
                    OldClockIn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    OldClockOut = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    NewClockIn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    NewClockOut = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftAdjustments_Employees_AdjustedById",
                        column: x => x.AdjustedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftAdjustments_ShiftRecords_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "ShiftRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ScheduleId",
                table: "Employees",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDays_ScheduleId",
                table: "ScheduleDays",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleOverrides_ScheduleId",
                table: "ScheduleOverrides",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAdjustments_AdjustedById",
                table: "ShiftAdjustments",
                column: "AdjustedById");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftAdjustments_ShiftId",
                table: "ShiftAdjustments",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftRecords_EmployeeId",
                table: "ShiftRecords",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Schedules_ScheduleId",
                table: "Employees",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Schedules_ScheduleId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "ScheduleDays");

            migrationBuilder.DropTable(
                name: "ScheduleOverrides");

            migrationBuilder.DropTable(
                name: "ShiftAdjustments");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "ShiftRecords");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ScheduleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Employees");
        }
    }
}
