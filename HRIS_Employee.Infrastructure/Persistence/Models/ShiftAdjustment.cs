using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class ShiftAdjustment
    {
        [Key]
        public int Id { get; set; }

        public int ShiftId { get; set; }

        public int AdjustedById { get; set; }

        public DateTimeOffset OldClockIn { get; set; }

        public DateTimeOffset OldClockOut { get; set; }

        public DateTimeOffset NewClockIn { get; set; }

        public DateTimeOffset NewClockOut { get; set; }

        public string? Reason { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public ShiftRecord Shift { get; set; } = default!;

        public Employee AdjustBy { get; set; } = default!;
    }
}
