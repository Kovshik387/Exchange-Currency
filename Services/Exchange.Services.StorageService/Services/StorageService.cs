﻿using Exchange.Services.StorageService.Infrastructure;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;

namespace Exchange.Services.StorageService.Services;

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;
    private readonly IMinioClientFactory _minioClientFactory;

    public StorageService(ILogger<StorageService> logger, IMinioClientFactory minioClientFactory)
    {
        _logger = logger;
        _minioClientFactory = minioClientFactory;
    }
    
    public async Task<string> AddProfilePhotoAsync(string userId, byte[] photo, string format)
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
            var existFile = minioClient.RemoveObjectsAsync(new RemoveObjectsArgs()
                .WithBucket(userId)
                .WithObject(userId));
        }

        var putObjectArgs = new PutObjectArgs()
                .WithBucket(userId)
                .WithStreamData(null)
                .WithObject(userId)
                .WithContentType(format)
                .WithObjectSize(photo.Length)
            ;
        
        await minioClient.PutObjectAsync(putObjectArgs);
        
        return null;
    }

    public async Task<string> DeleteProfilePhotoAsync(string userId)
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
            var existFile = minioClient.RemoveObjectsAsync(new RemoveObjectsArgs()
                .WithBucket(userId)
                .WithObject(userId));
        }
        
        return "";
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
                .WithExpiry(60 * 60)
            ;
        
        return await minioClient.PresignedGetObjectAsync(args);

    }
}