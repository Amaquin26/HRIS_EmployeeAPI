namespace HRIS_Employee.API.DTOs
{
    public record AuthUserDetailsDto
    {
        public int Id { get; internal set; }

        public string EntraObjectId { get; internal set; } = default!;

        public string EmployeeNumber { get; internal set; } = default!;

        public string FirstName { get; internal set; } = default!;

        public string LastName { get; internal set; } = default!;
    }
}
