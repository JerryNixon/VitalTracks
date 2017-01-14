using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendVitalTracks.UWP.Services
{
    public static class Settings
    {
        public static string HubName = "VitalTracksNotificationHub";
        public static string ListenerConnectionString = "Endpoint=sb://vitaltracksnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=oCbtDLkGMN6aXAmgstBd60K8xHa8KCpa53ttA6JWzfU=";
        public static string SenderConnectionString = "Endpoint=sb://vitaltracksnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=MgPlC3LnpaLBqnv4wMuAVN//kszNsZjRY33MIlK+p1Q=";
    }
}
