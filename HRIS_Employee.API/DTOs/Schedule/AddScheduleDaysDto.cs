namespace HRIS_Employee.API.DTOs.Schedule
{
    public class AddScheduleDaysDto
    {
        public int ScheduleId { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int BreakMinutes { get; set; }

        public List<DayOfWeek> RestDays { get; set; } = new List<DayOfWeek>();
    }
}
