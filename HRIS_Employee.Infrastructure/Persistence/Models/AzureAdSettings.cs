namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class AzureAdSettings
    {
        public string Instance { get; set; } = default!;

        public string TenantId { get; set; } = default!;

        public string ClientId { get; set; } = default!;

        public string ClientSecretKey { get; set; } = default!;

        public string Audience { get; set; } = default!;
    }
}
