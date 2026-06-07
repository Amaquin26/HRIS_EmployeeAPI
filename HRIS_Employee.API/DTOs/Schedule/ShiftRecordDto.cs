using HRIS_Employee.Infrastructure.Domain.Enums;

namespace HRIS_Employee.API.DTOs.Schedule
{
    public record ShiftRecordDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateOnly ShiftDate { get; set; }
        public DateTimeOffset ClockIn { get; set; }
        public DateTimeOffset? ClockOut { get; set; }
        public ShiftStatus Status { get; set; }
        public bool IsFlagged { get; set; }
        public string? FlagReason { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
