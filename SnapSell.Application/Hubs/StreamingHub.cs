using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;

namespace SnapSell.Application.Hubs
{
    public class MessageDto
    {
        public string Message { get; set; } = string.Empty;
        public string SenderUserName { get; set; } = string.Empty;
        public string StreamerId { get; set; } = string.Empty;

    }

    //public struct VideoData
    //{
    //    //public int Index { get; }
    //    public string Data { get; }

    //}
    public class StreamingHub : Hub
    {
        private readonly IWebHostEnvironment _env;

        public StreamingHub(IWebHostEnvironment env)
        {
            _env = env;

        }

        public async Task JoinGroup(string streamerId)
        {
            //TODO: use database to get connectionId using streamerId

            var streamerConId = streamerId;

            await Groups.AddToGroupAsync(Context.ConnectionId, streamerConId);
        }

        public async Task SendVideoData(IAsyncEnumerable<string> videoData)
        {
            //TODO: Save live in  files
            await foreach (var d in videoData)
            {

                await Clients.Group(Context.ConnectionId).SendAsync("video-data", d);
            }
        }

        public async Task SendMessage(MessageDto message)
        {

            //TODO: use database to get connectionId using streamerId
            var streamerconId = message.StreamerId;

            await Clients.Group(streamerconId).SendAsync("NewMessage", message);
        }
        public override async Task OnConnectedAsync()
        {
            //TODO:add connectionId to database
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            //TODO:remove connectionId from database

            await base.OnDisconnectedAsync(exception);
        }
    }
}
