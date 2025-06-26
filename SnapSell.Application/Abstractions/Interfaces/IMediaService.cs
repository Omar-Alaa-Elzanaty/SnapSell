using SnapSell.Domain.Dtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Application.Abstractions.Interfaces;

public interface IMediaService
{
    Task<string?> SaveAsync(MediaFileDto file, MediaTypes mediaTypes);
    void Delete(string file);
    Task<string?> UpdateAsync(MediaFileDto file, MediaTypes mediaType, string oldUrl);
    string? GetUrl(string? fileName, MediaTypes mediaType);
}