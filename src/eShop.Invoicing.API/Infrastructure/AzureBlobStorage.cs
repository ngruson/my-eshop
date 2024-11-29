using Azure.Storage.Blobs;
using eShop.Invoicing.API.Application.Storage;

namespace eShop.Invoicing.API.Infrastructure;

internal class AzureBlobStorage(IConfiguration configuration) : IFileStorage
{
    public async Task UploadFile(string fileName, byte[] bytes)
    {
        string connectionString = configuration["AzureBlobStorageConnectionString"]!;
        string containerName = "invoices";

        BlobServiceClient blobServiceClient = new(connectionString);
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync();
        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        using MemoryStream stream = new(bytes);
        await blobClient.UploadAsync(stream, overwrite: true);
    }
}
