using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendVitalTracks.Droid.Services
{
    public static class Settings
    {
        public static string HubName = "VitalTracksNotificationHub";
        public static string ListenerConnectionString = "Endpoint=sb://vitaltracksnamespace.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=oCbtDLkGMN6aXAmgstBd60K8xHa8KCpa53ttA6JWzfU=";

        //VERY VERY VERY IMPORTANT NOTE!!!!
        // Your package name MUST NOT start with an uppercase letter.
        // Android does not allow permissions to start with an upper case letter
        // If it does you will get a very cryptic error in logcat and it will not be obvious why you are crying!
        // So please, for the love of all that is kind on this earth, use a LOWERCASE first letter in your Package Name!!!!

        // https://console.developers.google.com/iam-admin/projects
        public static string GoogleProjectId = "ascendvitaltracks";
        public static string GoogleProjectNumber = "117016875425";

        // https://console.developers.google.com/apis/credentials
        public static string GoogleApiKey = "AIzaSyCLAJeYzRZp_YDxYR6GIGAYttHIyaZAGHc";
    }
}
