namespace HRIS_Employee.API.External.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        Task<(Stream Content, string ContentType, string FileName)?> DownloadFileAsync(string fileName);

        /// <summary>
        /// Generates a time-limited read-only SAS URL for a private blob stored
        /// in Azure Blob Storage. Intended for blobs that are strictly controlled
        /// and should not be publicly accessible.
        ///
        /// The generated URL grants temporary direct access to a single blob
        /// without exposing the storage account key.
        ///
        /// Requires the BlobServiceClient to be initialized using account-key
        /// authentication (for example via connection string). SAS generation
        /// is not supported when using Azure AD or Managed Identity credentials.
        /// </summary>
        /// <param name="fileName">
        /// The blob name or blob path inside the configured container.
        /// </param>
        /// <param name="expiryMinutes">
        /// The number of minutes before the generated SAS URL expires.
        /// Defaults to 60 minutes.
        /// </param>
        /// <returns>
        /// A signed read-only SAS URL that can be used to securely access
        /// the target blob for a limited time.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the current BlobClient cannot generate SAS URIs,
        /// typically because the client was authenticated using Azure AD
        /// or Managed Identity instead of an account key.
        /// </exception>
        string GenerateSasUrl(string fileName, int expiryMinutes = 60);
    }
}
