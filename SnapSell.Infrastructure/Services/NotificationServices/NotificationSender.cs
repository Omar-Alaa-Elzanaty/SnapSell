using FirebaseAdmin.Messaging;
using SnapSell.Application.Interfaces;



namespace SnapSell.Infrastructure.Services.NotificationServices
{
    public class NotificationSender : INotificationSender
    {
        /// <summary>
        /// Sends a notification to a specific FCM token
        /// </summary>
        public async Task<string> SendNotificationAsync(string fcmToken, string title, string body, Dictionary<string, string>? data = null)
        {
            var message = new Message
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? new Dictionary<string, string>()
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        /// <summary>
        /// Sends a notification to multiple FCM tokens
        /// </summary>
        public async Task<BatchResponse> SendNotificationAsync(List<string> fcmTokens, string title, string body, Dictionary<string, string>? data = null)
        {
            if (fcmTokens == null || fcmTokens.Count == 0)
                throw new ArgumentException("At least one FCM token must be provided", nameof(fcmTokens));

            var message = new MulticastMessage
            {
                Tokens = fcmTokens,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? new Dictionary<string, string>()
            };

            return await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }

        /// <summary>
        /// Sends a notification to a specific topic
        /// </summary>
        public async Task<string> SendNotificationToTopicAsync(string topic, string title, string body, Dictionary<string, string>? data = null)
        {
            var message = new Message
            {
                Topic = topic,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Data = data ?? new Dictionary<string, string>()
            };

            return await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        /// <summary>
        /// Sends a notification to multiple topics (sends separate messages to each topic)
        /// </summary>
        public async Task<BatchResponse> SendNotificationToTopicAsync(List<string> topics, string title, string body, Dictionary<string, string>? data = null)
        {
            if (topics == null || topics.Count == 0)
                throw new ArgumentException("At least one topic must be provided", nameof(topics));

            var messages = new List<Message>();

            foreach (var topic in topics)
            {
                messages.Add(new Message
                {
                    Topic = topic,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Data = data ?? new Dictionary<string, string>()
                });
            }

            return await FirebaseMessaging.DefaultInstance.SendEachAsync(messages);
        }

      
    }
}
