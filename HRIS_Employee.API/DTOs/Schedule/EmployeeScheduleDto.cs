namespace HRIS_Employee.API.DTOs.Schedule
{
    public class EmployeeScheduleDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int BreakMinutes { get; set; }

        public List<ScheduleDayDto> ScheduleDays { get; set; } = new List<ScheduleDayDto>();
    }
}
