using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Messaging;
using Windows.Networking.PushNotifications;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace AscendVitalTracks.UWP.Services
{
    public class SenderService
    {
        private NetworkAvailableHelper _network = new NetworkAvailableHelper();

        public bool IsSetup { get; private set; }

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

        public class NotificationHubSendHelper
        {
            private string[] parts;
            public NotificationHubSendHelper(string connectionString, string hubname)
            {
                HubName = hubname;
                ConnectionString = connectionString;
                parts = connectionString.Split(';');
                HubToken = new NotificationHub(hubname, connectionString).Token;
            }
            public string HubName { get; }
            public string ConnectionString { get; }
            public string Endpoint => $"https{parts.FirstOrDefault(x => x.StartsWith("Endpoint"))}";
            public string SharedAccessKeyName => parts.FirstOrDefault(x => x.StartsWith("SharedAccessKeyName"))?.Split('=')[1];
            public string SharedAccessKey => parts.FirstOrDefault(x => x.StartsWith("SharedAccessKey"))?.Split('=')[1];
            public string SasToken => GetSaSToken();
            public string HubToken { get; set; }
            public string Url => $"{Endpoint}{HubName}/messages/?api-version=2015-01";

            public enum Services
            {
                Android, Windows, Apple
            }

            public async Task SendAsync(string message, Services service = Services.Windows)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", SasToken);
                    client.DefaultRequestHeaders.Add("Content-Type", "multipart/mixed");

                    switch (service)
                    {
                        case Services.Android:
                            client.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "gcm");
                            break;
                        case Services.Windows:
                            client.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "windows");
                            client.DefaultRequestHeaders.Add("X-WNS-Type", "wns/raw");
                            break;
                        case Services.Apple:
                            client.DefaultRequestHeaders.Add("ServiceBusNotification-Format", "apple");
                            break;
                        default:
                            break;
                    }

                    var json = $"{{\"data\":{{\"message\":\"{message}\"}}";
                    var httpContent = new HttpStringContent(json, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

                    var result = await client.PostAsync(new Uri(Url), httpContent);
                    result.EnsureSuccessStatusCode();
                }
            }

            private string GetSaSToken(int minUntilExpire = 1000)
            {
                var targetUri = Uri.EscapeDataString(Url.ToLower()).ToLower();

                // Add an expiration in seconds to it.
                var expires_date = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + minUntilExpire * 60 * 1000;
                var expires_seconds = expires_date / 1000;
                var sign_this = targetUri + "\n" + expires_seconds;

                // Generate a HMAC-SHA256 hash or the uri and expiration using your secret key.
                var mac = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
                var encoding = BinaryStringEncoding.Utf8;
                var messageBuffer = CryptographicBuffer.ConvertStringToBinary(sign_this, encoding);
                var buffer = CryptographicBuffer.ConvertStringToBinary(SharedAccessKey, encoding);
                var key = mac.CreateKey(buffer);
                var signed = CryptographicEngine.Sign(key, messageBuffer);
                var signature = Uri.EscapeDataString(CryptographicBuffer.EncodeToBase64String(signed));
                return $"SharedAccessSignature sr={targetUri}&sig={signature}&se={expires_seconds}&skn={SharedAccessKeyName}";
            }
        }
    }
}
