using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;

namespace SnapSell.Application.Hubs
{
    public class MessageDto{
        public string? Message { get; set; }
        public string? Username { get; set; }

    }
    public struct VideoData
    {
        //public int Index { get; }
        public string Data { get; }

    }
    public class StreamingHub:Hub
    {
        private readonly IWebHostEnvironment _env;

        public StreamingHub(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task SendVideoData(IAsyncEnumerable<VideoData> videoData)
        {
            await foreach (var d in videoData)
            {
             
                await Clients.Others.SendAsync("video-data", d);
            }
        }

        public async Task SendMessage(MessageDto input)
        { 

            await Clients.Others.SendAsync("NewMessage", input);
        }
        public override async Task OnConnectedAsync()
        {
            
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}
