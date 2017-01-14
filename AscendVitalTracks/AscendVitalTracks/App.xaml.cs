using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AscendVitalTracks
{
    public partial class App : Application
    {
        // https://developer.xamarin.com/guides/xamarin-forms/dependency-service/introduction/
        PushNotificationLibrary.IPushNotificationService service
            = DependencyService.Get<PushNotificationLibrary.IPushNotificationService>();

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
            SetupPushNotificationsAsync();
        }

        private async void SetupPushNotificationsAsync()
        {
            await service.SetupAsync();
            Debug.WriteLine($"service.IsSetup={service.IsSetup}");

            if (service.IsSetup)
            {
                service.Subscribe(this, content =>
                {
                    Debug.WriteLine($"RawNotification={content.Content}");
                });
            }
        }

        protected override void OnStart()
        {
            // Handle when app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
