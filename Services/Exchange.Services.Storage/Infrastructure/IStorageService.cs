using Exchange.Services.Storage.Data.Responses;

namespace Exchange.Services.Storage.Infrastructure;

public interface IStorageService
{
    public Task<StorageResponse> AddProfilePhotoAsync(string userId, byte[] photo, string format);
    public Task<StorageResponse> DeleteProfilePhotoAsync(string userId);
    public Task<string> GetProfilePhotoAsync(string userId);
}