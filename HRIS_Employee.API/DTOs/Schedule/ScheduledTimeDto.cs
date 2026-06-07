namespace HRIS_Employee.API.DTOs.Schedule
{
    public record ScheduledTimeDto
    {
        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsRestDay { get; set; }
    }
}
