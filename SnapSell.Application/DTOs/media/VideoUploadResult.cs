namespace SnapSell.Application.DTOs.media;

public sealed class VideoUploadResult
{
    public string UploadId { get; set; }               // Upload session ID
    public string OriginalFileName { get; set; }       // Client's original filename
    public string SavedFileName { get; set; }          // "Guid.mp4" (what you want)
    public string FilePath { get; set; }               // Full system path
    public string PublicUrl { get; set; }              // Full public URL
    public List<int> ReceivedChunks { get; set; } = new List<int>();
    public int TotalChunks { get; set; }
    public bool IsComplete => ReceivedChunks.Count == TotalChunks && TotalChunks > 0;
    public List<(string fileName, string error)> FailedUploads { get; set; } = new List<(string, string)>();
}