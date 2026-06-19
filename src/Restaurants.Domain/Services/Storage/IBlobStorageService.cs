namespace Restaurants.Domain.Services.Storage;

public interface IBlobStorageService
{
    Task<string> UploadFileToBlobAsync(Stream data, string fileName);
    string? GetBlobSasUrl(string? blobUrl);
}