namespace HRIS_Employee.API.DTOs.Schedule
{
    public record EmployeeScheduleDetailDto
    {
        public ScheduledTimeDto? TodaySchedule { get; set; }
        public IReadOnlyList<ShiftHistoryDto> RecentShifts { get; set; } = new List<ShiftHistoryDto>();
    }
}
