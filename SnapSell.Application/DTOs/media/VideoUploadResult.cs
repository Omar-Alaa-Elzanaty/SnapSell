namespace SnapSell.Application.DTOs.media;

public sealed class VideoUploadResult
{
    public string UploadId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string PublicUrl { get; set; }
    public List<int> ReceivedChunks { get; set; } = new List<int>();
    public int TotalChunks { get; set; }
    public bool IsComplete => ReceivedChunks.Count == TotalChunks && TotalChunks > 0;
    public List<(string fileName, string error)> FailedUploads { get; set; } = new List<(string, string)>();
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }
    public TimeSpan? Duration => EndTime.HasValue ? EndTime - StartTime : null;
    public string RelativePath { get; set; }
}