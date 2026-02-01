namespace HRIS_Employee.API.DTOs
{
    public class WriteEmployeeDto
    {
        public string EntraObjectId { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string? ContactNumber { get; set; }

        public int EmployeeStatusId { get; set; }

        public DateTime HireDate { get; set; }
    }
}
