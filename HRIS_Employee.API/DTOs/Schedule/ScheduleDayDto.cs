namespace HRIS_Employee.API.DTOs.Schedule
{
    public class ScheduleDayDto
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public string DayName { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public bool CrossesMidnight { get; set; }

        public bool IsRestDay { get; set; }
    }
}
