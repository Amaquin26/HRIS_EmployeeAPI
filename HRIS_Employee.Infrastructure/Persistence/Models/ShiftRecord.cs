using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class ShiftRecord
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateOnly ShiftDate { get; set; }

        public DateTimeOffset ClockIn { get; set; }

        public DateTimeOffset ClockOut { get; set; }

        public int Status { get; set; }

        public bool IsFlag { get; set; } = false;

        public string? FlagReason { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public Employee Employee { get; set; } = default!;

        public ICollection<ShiftAdjustment> ShiftAdjustments { get; set; } = new List<ShiftAdjustment>();
    }
}
