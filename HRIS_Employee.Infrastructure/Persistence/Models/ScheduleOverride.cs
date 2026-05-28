using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class ScheduleOverride
    {
        [Key]
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public DateOnly SpecificDate { get; set; }

        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public int EndDayOffset { get; set; }

        public bool IsRestDay { get; set; }

        public string? Reason { get; set; }

        public Schedule Schedule { get; set; } = default!;
    }
}
