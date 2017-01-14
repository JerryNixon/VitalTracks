using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendVitalTracks.UWP.Services
{
    // Install-Package WindowsAzure.Messaging.Managed

    public class RawNotification
    {
        public string Content { get; internal set; }
    }

    public class PushNotificationSettings
    {
        public string HubName { get; set; }
        public string ConnectionString { get; set; }
    }

    // https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-windows-store-dotnet-get-started-wns-push-notification

    public class PushNotificationService
    {
        private static Windows.Networking.PushNotifications.PushNotificationChannel _channel;
        private static Microsoft.WindowsAzure.Messaging.NotificationHub _notificationHub;
        private static Microsoft.WindowsAzure.Messaging.Registration _registration;
        private PushNotificationSettings _settings;

        public event Windows.Foundation.TypedEventHandler<PushNotificationService, RawNotification> RawNotificationReceived;

        public PushNotificationService() : this(new PushNotificationSettings
        {
            HubName = "VitalTracksNotificationHub",
            ConnectionString = "Endpoint=sb://vitaltracksnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=oCbtDLkGMN6aXAmgstBd60K8xHa8KCpa53ttA6JWzfU=",
        })
        {
            // empty
        }

        public PushNotificationService(PushNotificationSettings settings)
        {
            _settings = settings;
        }

        public async Task<bool> InitializeAsync()
        {
            if (!string.IsNullOrEmpty(_registration?.RegistrationId))
            {
                return true;
            }
            _channel = await Windows.Networking.PushNotifications.PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            _channel.PushNotificationReceived += (s, e) =>
            {
                if (e.RawNotification != null)
                {
                    RawNotificationReceived?.Invoke(this, new RawNotification { Content = e.RawNotification.Content });
                };
            };
            _notificationHub = new Microsoft.WindowsAzure.Messaging.NotificationHub(_settings.HubName, _settings.ConnectionString);
            _registration = await _notificationHub.RegisterNativeAsync(_channel.Uri);
            return !string.IsNullOrEmpty(_registration.RegistrationId);
        }
    }
}
