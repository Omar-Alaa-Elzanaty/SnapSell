using Microsoft.AspNetCore.Http;
using SnapSell.Application.DTOs.media;

namespace SnapSell.Application.Interfaces;

public interface IVideoUploadService
{
    Task<VideoUploadResult> UploadChunkAsync(VideoChunkDto chunkDto);
    public string GetFullUrl(string relativePath);
}