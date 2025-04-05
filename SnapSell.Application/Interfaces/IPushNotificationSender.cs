
using FirebaseAdmin.Messaging;

namespace SnapSell.Application.Interfaces
{
    public interface IPushNotificationSender
    {
        Task<String> SendNotificationAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null);
        Task<BatchResponse> SendNotificationAsync(List<string> fcmTokens, string title, string body, Dictionary<string, string>? data = null);
        Task<String> SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string>? data = null);
        Task<BatchResponse> SendNotificationToTopicAsync(List<string> topics, string title, string body, Dictionary<string, string>? data = null);
    }
}
