using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Infrastructure.Services.MediaServices;

//public class LocalMediaService(
//    IWebHostEnvironment hostingEnvironment,
//    IConfiguration configuration)
//    : IMediaService
//{
//    public void Delete(string file)
//    {
//        var path = Path.Combine(hostingEnvironment.WebRootPath, file);

//        if (File.Exists(path))
//        {
//            File.Delete(path);
//        }
//    }

//    public string? GetUrl(string? fileUrl)
//    {
//        if (fileUrl.IsNullOrEmpty())
//            return null;

//        return configuration[""] + fileUrl;
//    }

//    public async Task<string?> SaveAsync(MediaFileDto media, MediaTypes mediaType)
//    {
//        if (media == null || media.Base64.IsNullOrEmpty())
//            return null;

//        var extension = Path.GetExtension(media.FileName);
//        var file = Guid.NewGuid().ToString() + extension;

//        var fileRootPath = GetFilePath(mediaType);

//        var path = Path.Combine("wwwroot",fileRootPath, file);

//        if (!Directory.Exists(path))
//        {
//            Directory.CreateDirectory(path);
//        }

//        await File.WriteAllBytesAsync(path, Convert.FromBase64String(media.Base64));

//        return Path.Combine(fileRootPath, file);
//    }

//    public async Task<string?> UpdateAsync(MediaFileDto media,MediaTypes mediaType, string oldUrl)
//    {
//        if (oldUrl == null && media == null)
//        {
//            return null;
//        }

//        if (media == null)
//        {
//            return oldUrl;
//        }

//        if (oldUrl == null)
//        {
//            return await SaveAsync(media, mediaType);
//        }

//        Delete(oldUrl);
//        return await SaveAsync(media, mediaType);
//    }

//    private string GetFilePath(MediaTypes mediaType)
//    {
//        return mediaType switch
//        {
//            MediaTypes.Image => configuration["MediaSavePath:ImagePath"]!,
//            MediaTypes.Video => configuration["MediaSavePath:VideoPath"]!,
//            _ => "",
//        };
//    }
//}

public sealed class LocalMediaService(
    IWebHostEnvironment hostingEnvironment,
    IConfiguration configuration,
    ILogger<LocalMediaService> logger)
    : IMediaService
{
    public void Delete(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(hostingEnvironment.WebRootPath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                logger.LogInformation("Deleted file: {FilePath}", fullPath);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
        }
    }

    public string? GetUrl(string? fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return null;

        var baseUrl = configuration["MediaBaseUrl"] ?? string.Empty;
        return $"{baseUrl.TrimEnd('/')}/{fileUrl.TrimStart('/')}";
    }

    public async Task<string?> SaveAsync(MediaFileDto media, MediaTypes mediaType)
    {
        try
        {

            var folderKey = mediaType == MediaTypes.Image ? "Images" : "Videos";
            var relativeFolderPath = configuration[$"MediaSavePaths:{folderKey}"] ??
                                     mediaType.ToString().ToLower() + "s";

            var fullFolderPath = Path.Combine(hostingEnvironment.WebRootPath, relativeFolderPath);

            Directory.CreateDirectory(fullFolderPath);
            logger.LogDebug("Ensured directory exists: {FolderPath}", fullFolderPath);

            var fileExtension = Path.GetExtension(media.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var relativePath = Path.Combine(relativeFolderPath, fileName).Replace("\\", "/");

            var fullPath = Path.Combine(hostingEnvironment.WebRootPath, relativePath);
            var fileBytes = Convert.FromBase64String(media.Base64);

            await File.WriteAllBytesAsync(fullPath, fileBytes);
            logger.LogInformation("Successfully saved {MediaType} to {Path}", mediaType, relativePath);

            return relativePath;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving {MediaType} file", mediaType);
            return null;
        }
    }

    public async Task<string?> UpdateAsync(MediaFileDto media, MediaTypes mediaType, string oldUrl)
    {
        try
        {
            if (media == null)
                return oldUrl;

            if (!string.IsNullOrEmpty(oldUrl))
                Delete(oldUrl);

            return await SaveAsync(media, mediaType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating media file");
            return null;
        }
    }
}