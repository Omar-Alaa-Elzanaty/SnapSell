using Microsoft.AspNetCore.Http;

namespace SnapSell.Application.DTOs.media;

public sealed class VideoChunkDto
{
    public string UploadId { get; set; } = Guid.NewGuid().ToString();
    public int ChunkNumber { get; set; }
    public int TotalChunks { get; set; }
    public string FileName { get; set; }
    public long TotalFileSize { get; set; }
    public IFormFile Chunk { get; set; }
}