using System.ComponentModel.DataAnnotations;

namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string EntraObjectId { get; set; } = default!;

        [Required]
        [MaxLength(50)]
        public string EmployeeNumber { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = default!;

        [MaxLength(150)]
        public string Email { get; set; } = default!;

        [MaxLength(15)]
        public string? ContactNumber { get; set; }

        [Required]
        public int EmployeeStatusId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public EmployeeStatus EmployeeStatus { get; set; } = default!;
    }
}
