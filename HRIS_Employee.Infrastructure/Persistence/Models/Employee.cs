using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        public string EntraObjectId { get; set; } = default!;

        [MaxLength(50)]
        public string EmployeeNumber { get; set; } = default!;

        [MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [MaxLength(100)]
        public string LastName { get; set; } = default!;

        [MaxLength(150)]
        public string Email { get; set; } = default!;

        [MaxLength(15)]
        public string? ContactNumber { get; set; }

        public int EmployeeStatusId { get; set; }

        public int ScheduleId { get; set; }

        public DateTimeOffset HiredDate { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedAt { get; set; }

        public string TimeZone { get; set; } = null!;

        public EmployeeStatus EmployeeStatus { get; set; } = default!;

        public Schedule Schedule { get; set; } = default!;

        public ICollection<ShiftRecord> ShiftRecords { get; set; } = new List<ShiftRecord>();

        public ICollection<ShiftAdjustment> ShiftAdjustments { get; set; } = new List<ShiftAdjustment>();
    }
}
