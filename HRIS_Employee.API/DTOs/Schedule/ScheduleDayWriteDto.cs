namespace HRIS_Employee.API.DTOs.Schedule
{
    public record ScheduleDayWriteDto
    {
        public int Id { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public bool CrossesMidnight { get; set; }

        public bool IsRestDay { get; set; }
    }
}
