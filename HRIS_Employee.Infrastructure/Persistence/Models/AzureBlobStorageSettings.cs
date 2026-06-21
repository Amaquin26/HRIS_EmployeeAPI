namespace HRIS_Employee.Infrastructure.Persistence.Models
{
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; } = default!;

        public string ContainerName { get; set; } = default!;
    }
}
