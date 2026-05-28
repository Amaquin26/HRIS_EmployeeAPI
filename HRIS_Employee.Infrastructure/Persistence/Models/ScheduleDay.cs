using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class ScheduleDay
    {
        [Key]
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public bool CrossesMidnight { get; set; }

        public bool IsRestDay { get; set; }

        public Schedule Schedule { get; set; } = default!;
    }
}
