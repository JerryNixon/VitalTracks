using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Messaging;
using PushNotificationLibrary;
using Windows.Networking.PushNotifications;

namespace AscendVitalTracks.UWP.Services
{
    // Install-Package WindowsAzure.Messaging.Managed
    // https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-windows-store-dotnet-get-started-wns-push-notification

    public class ListenerService : ListenerBase
    {
        private NetworkAvailableHelper _network = new NetworkAvailableHelper();

        public override async Task<bool> SetupAsync()
        {
            // is this even possible?
            if (IsSetup) return true;
            if (!await _network.IsInternetAvailable()) return false;

            // register the channel
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var hub = new NotificationHub(Settings.HubName, Settings.ListenerConnectionString);
            var registration = await hub.RegisterNativeAsync(channel.Uri);

            // now setup listening
            if (IsSetup = !string.IsNullOrEmpty(registration?.RegistrationId))
            {
                channel.PushNotificationReceived += (s, e) =>
                {
                    if (e.RawNotification != null)
                    {
                        Send(e.RawNotification.Content);
                    };
                };
                return true;
            }
            return false;
        }
    }
}