using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SnapSell.Application.Abstractions.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Infrastructure.Services.MediaServices;

public sealed class MediaService(
    IWebHostEnvironment hostingEnvironment,
    IConfiguration configuration) : IMediaService
{
    private readonly string _mediaBasePath = hostingEnvironment.WebRootPath;
    private readonly string _baseUrl = configuration["MediaBaseUrl"]?.TrimEnd('/') ?? string.Empty;
    private string GetMediaFolder(MediaTypes mediaType) => mediaType == MediaTypes.Image ? "images" : "videos";

    public void Delete(string filePath)
    {
        var fullPath = Path.Combine(_mediaBasePath, filePath.TrimStart('/'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public string? GetUrl(string? fileName, MediaTypes mediaType)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;

        var folder = GetMediaFolder(mediaType);
        return $"{_baseUrl}/{folder}/{fileName.TrimStart('/')}";
    }

    public async Task<string?> SaveAsync(MediaFileDto media, MediaTypes mediaType)
    {
        var folder = GetMediaFolder(mediaType);
        var fullFolderPath = Path.Combine(_mediaBasePath, folder);
        Directory.CreateDirectory(fullFolderPath);

        var fileExtension = GetMediaExtension(media.Base64);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var fullPath = Path.Combine(fullFolderPath, fileName);

        await File.WriteAllBytesAsync(fullPath, Convert.FromBase64String(media.Base64));
        return fileName;
    }

    public async Task<string?> UpdateAsync(MediaFileDto media, MediaTypes mediaType, string oldUrl)
    {
        if (media == null)
            return oldUrl;

        if (!string.IsNullOrEmpty(oldUrl))
            Delete(oldUrl);

        return await SaveAsync(media, mediaType);
    }

    private string GetMediaExtension(string base64)
    {
        if (string.IsNullOrEmpty(base64))
            return null;

        // Remove data URI prefix if present
        var cleanBase64 = base64;
        if (base64.StartsWith("data:image/"))
        {
            var commaIndex = base64.IndexOf(',');
            if (commaIndex > 0)
            {
                cleanBase64 = base64.Substring(commaIndex + 1);
            }
        }

        try
        {
            var bytes = Convert.FromBase64String(cleanBase64.Length > 12 ?
                        cleanBase64.Substring(0, 12) : cleanBase64);

            if (bytes.Length >= 8 && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return "png";
            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xD8)
                return "jpg";
            if (bytes.Length >= 6 && bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return "gif";
            if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D)
                return "bmp";
            if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0x01 && bytes[3] == 0x00)
                return "ico";
            if (bytes.Length >= 12 && bytes[0] == 0x52 && bytes[1] == 0x49 && bytes[2] == 0x46 && bytes[3] == 0x46 &&
                bytes[8] == 0x57 && bytes[9] == 0x45 && bytes[10] == 0x42 && bytes[11] == 0x50)
                return "webp";

            return ""; 
        }
        catch
        {
            return "";
        }
    }
}