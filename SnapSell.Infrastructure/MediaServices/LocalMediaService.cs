using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SnapSell.Application.Interfaces;
using SnapSell.Domain.Dtos;
using SnapSell.Domain.Enums;

namespace SnapSell.Infrastructure.MediaServices
{
    public class LocalMediaService : IMediaService
    {
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _configuration;

        public LocalMediaService(
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _host = hostingEnvironment;
            _configuration = configuration;
        }

        public void Delete(string file)
        {
            var path = Path.Combine(_host.WebRootPath, file);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task<string?> SaveAsync(MediaFileDto media, MediaTypes mediaType)
        {
            if (media == null || media.Base64.IsNullOrEmpty())
                return null;

            var extension = Path.GetExtension(media.FileName);
            var file = Guid.NewGuid().ToString() + extension;

            var fileRootPath = GetFilePath(mediaType);

            var path = Path.Combine("wwwroot",fileRootPath, file);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await File.WriteAllBytesAsync(path, Convert.FromBase64String(media.Base64));

            return Path.Combine(fileRootPath, file);
        }

        public async Task<string?> UpdateAsync(MediaFileDto media,MediaTypes mediaType, string oldUrl)
        {
            if (oldUrl == null && media == null)
            {
                return null;
            }

            if (media == null)
            {
                return oldUrl;
            }

            if (oldUrl == null)
            {
                return await SaveAsync(media, mediaType);
            }

            Delete(oldUrl);
            return await SaveAsync(media, mediaType);
        }

        private string GetFilePath(MediaTypes mediaType)
        {
            return mediaType switch
            {
                MediaTypes.Image => _configuration["MediaSavePath:ImagePath"]!,
                _ => "",
            };
        }
    }
}
