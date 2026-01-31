namespace HRIS_Employee.API.DTOs
{
    public class PaginatedItemsDto<T>
    {
        public int TotalRecords { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public string? SkipToken { get; set; }
    }
}
