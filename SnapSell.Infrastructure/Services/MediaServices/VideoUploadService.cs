using System.Collections.Concurrent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnapSell.Application.DTOs.media;
using SnapSell.Application.Interfaces;

namespace SnapSell.Infrastructure.Services.MediaServices;

public sealed class VideoUploadService : IVideoUploadService
{
    private readonly string _tempPath;
    private readonly string _videoPath;
    private readonly string _baseUrl;
    private readonly ILogger<VideoUploadService> _logger;
    private static readonly ConcurrentDictionary<string, List<int>> ChunkTracker = new();
    public VideoUploadService(
        IConfiguration configuration,
        IWebHostEnvironment hostingEnvironment,
        ILogger<VideoUploadService> logger)
    {
        _logger = logger;
        _baseUrl = configuration["MediaBaseUrl"]?.TrimEnd('/') ?? string.Empty;
        _tempPath = Path.Combine(hostingEnvironment.WebRootPath, "uploads/temp");
        _videoPath = Path.Combine(hostingEnvironment.WebRootPath, configuration["MediaSavePaths:Videos"] ?? "videos");

        Directory.CreateDirectory(_tempPath);
        Directory.CreateDirectory(_videoPath);
    }
    public string GetFullUrl(string relativePath)
    {
        if (string.IsNullOrEmpty(relativePath))
            return string.Empty;

        // Handle both forward and backward slashes
        var cleanPath = relativePath.Replace('\\', '/').TrimStart('/');
        return $"{_baseUrl}/{cleanPath}";
    }

    public async Task<VideoUploadResult> UploadChunkAsync(VideoChunkDto chunkDto)
    {
        var result = new VideoUploadResult
        {
            UploadId = chunkDto.UploadId,
            FileName = chunkDto.FileName,
            TotalChunks = chunkDto.TotalChunks
        };

        try
        {
            // Save chunk
            var chunkFile = Path.Combine(_tempPath, $"{chunkDto.UploadId}_{chunkDto.ChunkNumber}");
            await SaveChunkAsync(chunkDto.Chunk, chunkFile);

            // Track progress
            UpdateChunkTracking(chunkDto.UploadId, chunkDto.ChunkNumber);
            result.ReceivedChunks = GetReceivedChunks(chunkDto.UploadId);

            // Assemble if complete
            if (result.IsComplete)
            {
                result.FilePath = await AssembleVideoAsync(chunkDto);
                result.PublicUrl = $"{_baseUrl}/{Path.GetRelativePath(_videoPath, result.FilePath).Replace('\\', '/')}";
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

    private async Task<string> AssembleVideoAsync(VideoChunkDto chunkDto)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(chunkDto.FileName)}";
        var finalPath = Path.Combine(_videoPath, fileName);

        await using var finalStream = File.Create(finalPath);
        for (int i = 1; i <= chunkDto.TotalChunks; i++)
        {
            var chunkFile = Path.Combine(_tempPath, $"{chunkDto.UploadId}_{i}");
            await using var chunkStream = File.OpenRead(chunkFile);
            await chunkStream.CopyToAsync(finalStream);
        }

        return finalPath;
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