using Exchange.Services.StorageService.Infrastructure;
using Grpc.Core;
using StorageServiceProto;

namespace Exchange.ExchangeStorage.Services;

public class StorageService : StorageSerive.StorageSeriveBase
{
    private readonly ILogger<StorageService> _logger;
    private readonly IStorageService _storageSerivce;
    public StorageService(IStorageService storageSerivce, ILogger<StorageService> logger)
    {
        _storageSerivce = storageSerivce;
        _logger = logger;
    }

    public override async Task<StorageResponse> UploadImage(UploadImageRequest request, ServerCallContext context)
    {
        var response = await _storageSerivce.AddProfilePhotoAsync(request.UserId, request.Image.ToByteArray(), request.ImageFormat);

        return new StorageResponse()
        {
            Success = response,
        };
    }

    public override Task<StorageResponse> DeleteImage(DeleteImageRequest request, ServerCallContext context)
    {
        return base.DeleteImage(request, context);
    }

    public override async Task<StorageResponse> GetImage(GetImageRequest request, ServerCallContext context)
    {
        var response = await _storageSerivce.GetProfilePhotoAsync(request.UserId);
        return new StorageResponse()
        {
            Success = true,
            Url = response
        };
    }
}
