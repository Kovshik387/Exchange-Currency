namespace Exchange.Services.StorageService.Infrastructure;

public interface IStorageService
{
    public Task<bool> AddProfilePhotoAsync(string userId, byte[] photo, string format);
    public Task<bool> DeleteProfilePhotoAsync(string userId);
    public Task<string> GetProfilePhotoAsync(string userId);
}