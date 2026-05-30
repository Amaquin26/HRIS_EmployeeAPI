namespace HRIS_Employee.API.DTOs.Schedule
{
    public class ScheduleDayDto
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public string DayName { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool CrossesMidnight { get; set; }

        public bool IsRestDay { get; set; }
    }
}
