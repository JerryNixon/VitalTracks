using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AscendVitalTracks.UWP
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Xamarin.Forms.Forms.Init(e);
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            Window.Current.Activate();
            SetupPushNotifications();
        }

        private async void SetupPushNotifications()
        {
            var service = new Services.PushNotificationService();
            var result = await service.InitializeAsync();
            await new Windows.UI.Popups.MessageDialog(result.ToString()).ShowAsync();
            var dispatcher = Window.Current.Dispatcher;
            service.RawNotificationReceived += async (s, e) =>
            {
                await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await new Windows.UI.Popups.MessageDialog(e.Content).ShowAsync();
                });
            };
        }
    }
}
