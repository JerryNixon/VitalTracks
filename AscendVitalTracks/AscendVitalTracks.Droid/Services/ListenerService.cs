using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Messaging;
using PushNotificationLibrary;
using Windows.Networking.PushNotifications;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace AscendVitalTracks.Droid.Services
{
    // Install-Package WindowsAzure.Messaging.Managed
    // https://docs.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm

    public class ListenerService : ListenerBase
    {
        public override async Task<bool> SetupAsync()
        {
            // is this even possible?
            if (IsSetup) return true;

            // Initialize our Gcm Service Hub
            SampleGcmService.Initialize(this);

            // Register for GCM
            SampleGcmService.Register(this);

            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            Log.Info("MainActivity", "Registering...");
            GcmClient.Register(this, Settings.GoogleProjectId);

            /*

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

            */
        }
    }

    // https://docs.microsoft.com/en-us/azure/notification-hubs/xamarin-notification-hubs-push-notifications-android-gcm
    // https://github.com/xamarin/mobile-samples/blob/master/Azure/NotificationHubs/Android/XamarinTodoQuickStart/GcmService.cs
    // https://components.xamarin.com/gettingstarted/azure-messaging
}