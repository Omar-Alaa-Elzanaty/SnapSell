using System.Collections.Concurrent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnapSell.Application.DTOs.media;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Infrastructure.Services.MediaServices;

public sealed class MediaService : IMediaService
{
    private readonly string _tempPath;
    private readonly string _mediaBasePath;
    private readonly string _baseUrl;
    private readonly ILogger<MediaService> _logger;
    private static readonly ConcurrentDictionary<string, List<int>> ChunkTracker = new();

    public MediaService(
        IWebHostEnvironment hostingEnvironment,
        IConfiguration configuration,
        ILogger<MediaService> logger)
    {
        _logger = logger;
        _baseUrl = configuration["MediaBaseUrl"]?.TrimEnd('/') ?? string.Empty;
        _mediaBasePath = hostingEnvironment.WebRootPath;
        _tempPath = Path.Combine(_mediaBasePath, "uploads/temp");

        Directory.CreateDirectory(_tempPath);
    }

    public void Delete(string filePath)
    {
        try
        {
            var fullPath = Path.Combine(_mediaBasePath, filePath.TrimStart('/'));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("Deleted file: {FilePath}", fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file: {FilePath}", filePath);
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
        try
        {
            var folder = GetMediaFolder(mediaType);
            var fullFolderPath = Path.Combine(_mediaBasePath, folder);
            Directory.CreateDirectory(fullFolderPath);

            var fileExtension = Path.GetExtension(media.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var fullPath = Path.Combine(fullFolderPath, fileName);

            await File.WriteAllBytesAsync(fullPath, Convert.FromBase64String(media.Base64));
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving {MediaType} file", mediaType);
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
            _logger.LogError(ex, "Error updating media file");
            return null;
        }
    }

    public async Task<VideoUploadResult> UploadChunkAsync(VideoChunkDto chunkDto)
    {
        var result = new VideoUploadResult
        {
            UploadId = chunkDto.UploadId,
            OriginalFileName = chunkDto.FileName,
            TotalChunks = chunkDto.TotalChunks
        };

        try
        {
            // Save chunk to temp location
            var chunkFile = Path.Combine(_tempPath, $"{chunkDto.UploadId}_{chunkDto.ChunkNumber}");
            await SaveChunkAsync(chunkDto.Chunk, chunkFile);

            // Track upload progress
            UpdateChunkTracking(chunkDto.UploadId, chunkDto.ChunkNumber);
            result.ReceivedChunks = GetReceivedChunks(chunkDto.UploadId);

            // Process when all chunks are received
            if (result.IsComplete)
            {
                // Assemble final video file (creates Guid.mp4 in videos folder)
                result.FilePath = await AssembleVideoAsync(chunkDto);

                // Set the important filename properties
                result.SavedFileName = Path.GetFileName(result.FilePath); // Just "Guid.mp4"
                result.OriginalFileName = chunkDto.FileName; // Original client filename
                result.PublicUrl = GetUrl(result.SavedFileName, MediaTypes.Video); // Full public URL

                // Cleanup temp files
                CleanTempFiles(chunkDto.UploadId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Chunk upload failed for {FileName}", chunkDto.FileName);
            result.FailedUploads.Add((chunkDto.FileName, ex.Message));
            return result;
        }
    }

    private async Task<string> AssembleVideoAsync(VideoChunkDto chunkDto)
    {
        var folder = GetMediaFolder(MediaTypes.Video);
        var fullFolderPath = Path.Combine(_mediaBasePath, folder);
        Directory.CreateDirectory(fullFolderPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(chunkDto.FileName)}";
        var finalPath = Path.Combine(fullFolderPath, fileName);

        await using var finalStream = File.Create(finalPath);
        for (int i = 1; i <= chunkDto.TotalChunks; i++)
        {
            var chunkFile = Path.Combine(_tempPath, $"{chunkDto.UploadId}_{i}");
            await using var chunkStream = File.OpenRead(chunkFile);
            await chunkStream.CopyToAsync(finalStream);
        }

        return finalPath;
    }

    private string GetMediaFolder(MediaTypes mediaType)
    {
        return mediaType == MediaTypes.Image ? "images" : "videos";
    }

    private async Task SaveChunkAsync(IFormFile chunk, string savePath)
    {
        await using var stream = File.Create(savePath);
        await chunk.CopyToAsync(stream);
    }

    private void UpdateChunkTracking(string uploadId, int chunkNumber)
    {
        ChunkTracker.AddOrUpdate(uploadId,
            _ => new List<int> { chunkNumber },
            (_, list) =>
            {
                list.Add(chunkNumber);
                return list;
            });
    }

    private List<int> GetReceivedChunks(string uploadId)
    {
        return ChunkTracker.TryGetValue(uploadId, out var chunks)
            ? chunks.ToList()
            : new List<int>();
    }

    private void CleanTempFiles(string uploadId)
    {
        try
        {
            foreach (var file in Directory.EnumerateFiles(_tempPath, $"{uploadId}_*"))
            {
                File.Delete(file);
            }

            ChunkTracker.TryRemove(uploadId, out _);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Temp cleanup failed for {UploadId}", uploadId);
        }
    }
}