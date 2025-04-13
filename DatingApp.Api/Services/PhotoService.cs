using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using Microsoft.Extensions.Options;

namespace DatingApp.Api.Services;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    public PhotoService(IOptions<CloudinarySettings> config)
    {
        var cloudinaryConfig = config.Value;
        var account = new Account(
            cloudinaryConfig.CloudName,
            cloudinaryConfig.ApiKey,
            cloudinaryConfig.ApiSecret
        );
        _cloudinary = new Cloudinary(account);
    }

    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();

        if(file.Length > 0)
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500)
                    .Width(500).Crop("fill").Gravity("face"),
                Folder = "datingapp"
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParams);
        }

        return uploadResult;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);

        return await _cloudinary.DestroyAsync(deleteParams);
    }
}