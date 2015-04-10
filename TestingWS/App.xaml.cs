using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Microsoft.WindowsAzure.MobileServices;
using TestingApp.DataModels;
using TestingApp.Models;
using Windows.UI.Popups;
using Microsoft.Kinect.Xaml.Controls;


namespace TestingWS
{
    
    sealed partial class App : Application
    {
       
        public static TelemetryClient TelemetryClient;



        //Declerations
        public static MobileServiceClient MobileService = new MobileServiceClient(
       "https://gymnasio.azure-mobile.net/",
       "hyLxEreGFGZwcHxFQNQmccdTAAbVEa32"
   );
        public static MobileServiceCollection<User, User> _userData;
        public static User _AppUser;
        public static Post _PostData;
        public static User _SignUpUser;
        public static Article _SelectedArticle;
        public static string ExcerciseType;
        public static Session SessionData;
        public static TimeSpan SessionTime;
        public static bool _isAchieved;
        public static string _nameOfAchi;
        public static Achievements _achi;
        public static bool IsSessionDataAvailable = false;




        public static async void MessageCustom(string title, string details)
        {

            await new MessageDialog(details, title).ShowAsync();
        }
    




        public App()
        {
            TelemetryClient = new TelemetryClient();

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }


                KinectRegion kinectRegion = new KinectRegion();
                KinectUserViewer kinectUserViewer = new KinectUserViewer()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 100,
                    Width = 121
                };
                Grid grd = new Grid();
                grd.Children.Add(kinectRegion);
                grd.Children.Add(kinectUserViewer);
                kinectRegion.Content = rootFrame;

                Window.Current.Content = grd;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
