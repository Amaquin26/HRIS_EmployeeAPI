using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using HRIS_Employee.Infrastructure.Persistence.Models;
using Microsoft.Extensions.Options;

namespace HRIS_Employee.API.External.BlobStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IOptions<AzureBlobStorageSettings> azureBlobStorageSettings)
        {
            var connectionString = azureBlobStorageSettings.Value.ConnectionString;
            _containerName = azureBlobStorageSettings.Value.ContainerName;
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty or null.");

            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var extension = Path.GetExtension(file.FileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

            var uniqueFileName = $"{fileNameWithoutExtension}_{Guid.NewGuid()}{extension}";

            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });
            }

            return blobClient.Uri.ToString();
        }

        public async Task<(Stream Content, string ContentType, string FileName)?> DownloadFileAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (!await blobClient.ExistsAsync())
                return null;

            var downloadResult = await blobClient.DownloadContentAsync();
            var contentType = downloadResult.Value.Details.ContentType ?? "application/octet-stream";
            var stream = new MemoryStream(downloadResult.Value.Content.ToArray());

            return (stream, contentType, fileName);
        }

        /// <inheritdoc />
        public string GenerateSasUrl(string fileName, int expiryMinutes = 60)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (!blobClient.CanGenerateSasUri)
                throw new InvalidOperationException(
                    "SAS generation requires account-key auth (a connection string). It won't work with Azure AD / managed identity credentials.");

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
    }
}
