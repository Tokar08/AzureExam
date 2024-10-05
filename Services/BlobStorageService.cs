using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CarsAzureExam.Interfaces;

namespace CarsAzureExam.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;

    public BlobStorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string> UploadFileAsync(IFormFile file, string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
        
        var blobClient = containerClient.GetBlobClient(file.FileName);
        await using var stream = file.OpenReadStream();
        
        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType});
        
        return blobClient.Uri.ToString();
    }

    public async Task DeleteFileAsync(string fileUrl, string containerName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(Path.GetFileName(fileUrl));
        await blobClient.DeleteIfExistsAsync();
    }
}