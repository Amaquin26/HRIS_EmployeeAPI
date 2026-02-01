namespace HRIS_Employee.API.DTOs
{
    public class PaginationQueryDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? SearchTerm { get; set; }

        public string? SkipToken { get; set; }
    }
}
