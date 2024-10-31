using Exchange.Services.Storage.Data.Responses;
using Exchange.Services.Storage.Infrastructure;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Exchange.Services.Storage.Services;

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;
    private readonly IMinioClientFactory _minioClientFactory;

    public StorageService(ILogger<StorageService> logger, IMinioClientFactory minioClientFactory)
    {
        _logger = logger;
        _minioClientFactory = minioClientFactory;
    }
    
    public async Task<StorageResponse> AddProfilePhotoAsync(string userId, byte[] photo, string format)
    {
        try
        {
            using var minioClient = _minioClientFactory.CreateClient();

            if (!await minioClient.BucketExistsAsync(new BucketExistsArgs()
                    .WithBucket(userId)))
            {
                _logger.LogInformation($"Creating bucket {userId}");
                await minioClient.MakeBucketAsync(new MakeBucketArgs()
                    .WithBucket(userId));
            }
            else
            {
                _logger.LogInformation($"Remove old object {userId}");
                await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                    .WithBucket(userId)
                    .WithObject(userId)
                );
            }

            var putObjectArgs = new PutObjectArgs()
                    .WithBucket(userId)
                    .WithStreamData(new MemoryStream(photo))
                    .WithObject(userId)
                    .WithContentType(format)
                    .WithObjectSize(photo.Length)
                ;
        
            await minioClient.PutObjectAsync(putObjectArgs);
        
            return new StorageResponse()
            {
                Success = true,
                Url = await this.GetProfilePhotoAsync(userId)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new StorageResponse()
            {
                Success = false,
                ErrorMessage = $"An error occurred when creating profile photo: {ex.Message}"
            };
        }
    }
    //TODO Custom exception
    public async Task<StorageResponse> DeleteProfilePhotoAsync(string userId)
    {
        using var minioClient = _minioClientFactory.CreateClient();

        if (!await minioClient.BucketExistsAsync(new BucketExistsArgs()
                .WithBucket(userId)))
        {
            return new StorageResponse()
            {
                Success = false,
                ErrorMessage = "Bucket does not exist"
            };
        }

        await minioClient.RemoveObjectsAsync(new RemoveObjectsArgs()
                    .WithBucket(userId)
                    .WithObject(userId));
        return new StorageResponse()
        {
            Success = true
        };
    }

    public async Task<string> GetProfilePhotoAsync(string userId)
    {
        using var minioClient = _minioClientFactory.CreateClient();

        if (!await minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(userId)))
            return "";
        
        //TODO configuration time
        var args = new PresignedGetObjectArgs()
                .WithBucket(userId)
                .WithObject(userId)
                .WithExpiry(60 * 60 * 60)
            ;
        
        return await minioClient.PresignedGetObjectAsync(args);

    }
}