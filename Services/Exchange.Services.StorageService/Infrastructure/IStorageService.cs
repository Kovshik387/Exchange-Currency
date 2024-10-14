namespace Exchange.Services.StorageService.Infrastructure;

public interface IStorageService
{
    public Task<string> AddProfilePhotoAsync(string userId, byte[] photo, string format);
    public Task<string> DeleteProfilePhotoAsync(string userId);
    public Task<string> GetProfilePhotoAsync(string userId);
}