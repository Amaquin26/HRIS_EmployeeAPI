namespace HRIS_Employee.API.DTOs
{
    public record EntraUserDto
    {
        public string ObjectId { get; set; } = default!;

        public string DisplayName { get; set; } = default!;

        public string? GivenName { get; set; }

        public string? Surname { get; set; }

        public string? JobTitle { get; set; }

        public string? MobilePhone { get; set; }

        public string? Email { get; set; }

        public string? OfficeLocation { get; set; }
    }
}
