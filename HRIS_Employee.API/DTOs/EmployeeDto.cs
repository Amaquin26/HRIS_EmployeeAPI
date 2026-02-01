namespace HRIS_Employee.API.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string EntraObjectId { get; set; } = default!;

        public string EmployeeNumber { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string? ContactNumber { get; set; } = default!;

        public int EmployeeStatusId { get; set; }

        public string EmployeeStatusName { get; set; } = default!;

        public DateTime HireDate { get; set; }
    }
}