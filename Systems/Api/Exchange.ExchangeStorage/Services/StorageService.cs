using Exchange.Services.Storage.Infrastructure;
using Grpc.Core;
using StorageServiceProto;

namespace Exchange.ExchangeStorage.Services;

public class StorageService : StorageServiceProto.StorageService.StorageServiceBase
{
    private readonly ILogger<StorageService> _logger;
    private readonly IStorageService _storageService;
    public StorageService(IStorageService storageService, ILogger<StorageService> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public override async Task<StorageResponse> UploadImage(UploadImageRequest request, ServerCallContext context)
    {
        _logger.LogInformation("UploadImage");
        
        var response = await _storageService.AddProfilePhotoAsync(request.UserId, request.Image.ToByteArray(), request.ImageFormat);

        return new StorageResponse()
        {
            Success = response.Success,
            Url = response.Url,
            ErrorMessage = response.ErrorMessage,
        };
    }

    public override async Task<StorageResponse> DeleteImage(DeleteImageRequest request, ServerCallContext context)
    {
        _logger.LogInformation("DeleteImage");
        
        var response = await _storageService.DeleteProfilePhotoAsync(request.UserId);
        return new StorageResponse()
        {
            Success = response.Success,
            Url = response.Url,
            ErrorMessage = response.ErrorMessage,
        };
    }

    public override async Task<StorageResponse> GetImage(GetImageRequest request, ServerCallContext context)
    {
        _logger.LogInformation("GetImage");
        
        var response = await _storageService.GetProfilePhotoAsync(request.UserId);
        return new StorageResponse()
        {
            Success = true,
            Url = response
        };
    }
}
