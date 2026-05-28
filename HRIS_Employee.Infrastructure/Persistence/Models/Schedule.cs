using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public int BreakMinutes { get; set; } = 60;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<ScheduleDay> ScheduleDays { get; set; } = new List<ScheduleDay>();

        public ICollection<ScheduleOverride> ScheduleOverrides { get; set; } = new List<ScheduleOverride>();
    }
}
