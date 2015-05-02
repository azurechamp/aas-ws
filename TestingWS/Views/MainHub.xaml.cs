using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestingApp.DataModels;
using TestingApp.Models;
using Windows.ApplicationModel.Store;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace TestingWS.Views
{
    public sealed partial class MainHub : Page
    {
        #region Declarations

        private ObservableCollection<Achievements> obs_achvments = new ObservableCollection<Achievements>();
        private ObservableCollection<vene> obs_NearbyParks = new ObservableCollection<vene>();
        private ObservableCollection<Article> obs_Articles = new ObservableCollection<Article>();
        private MobileServiceCollection<Post, Post> items;
        private IMobileServiceTable<Post> todoTable = App.MobileService.GetTable<Post>();
        private MobileServiceCollection<Achievements, Achievements> Achitems;
        private IMobileServiceTable<Achievements> achivTable = App.MobileService.GetTable<Achievements>();
        double latitude, longitude;
        #endregion
        public MainHub()
        {
            this.InitializeComponent();
            GetArticlesData();

            //GetNearbyData();
            
            GetLocation();
            lbx_articles.SelectionChanged += lbx_articles_SelectionChanged;
            lbx_nearby.SelectionChanged += lbx_nearby_SelectionChanged;
            lbx_Posts.SelectionChanged+=lbx_Posts_SelectionChanged;
        }
        #region Selection Changed
        async void lbx_nearby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            try
            {
                string uriToLaunch = @"bingmaps:?cp=" + (lbx_nearby.SelectedItem as vene).lat + "~-" + (lbx_nearby.SelectedItem as vene).lng + "&lvl=10";
                var uri = new Uri(uriToLaunch);

                // Launch the URI
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);
                if (success)
                {
                    // URI launched
                }
                else
                {
                    // URI launch failed
                }
            }
            catch (Exception exc) 
            {

                Debug.WriteLine(exc.Message);
            }

        }
        void lbx_articles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App._SelectedArticle = lbx_articles.SelectedItem as Article;
            if (lbx_articles.SelectedIndex == -1)
            {
            }
            else
            {
                this.Frame.Navigate(typeof(ViewArticle));
                lbx_articles.SelectedIndex = -1;
            }
        }
        void lbx_Posts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App._PostData = lbx_Posts.SelectedItem as Post;
            if (lbx_Posts.SelectedIndex == -1)
            {

            }
            else
            {
               this.Frame.Navigate(typeof(PostView));
                lbx_Posts.SelectedIndex = -1;
            }

        }
        private async void GetLocation()
        {
            Geolocator geo = new Geolocator();
            // await this because we don't know hpw long it will take to complete and we don't want to block the UI
           geo.DesiredAccuracyInMeters = 50;

           try
           {
               Geoposition pos = await geo.GetGeopositionAsync(); // get the raw geoposition data

               latitude = pos.Coordinate.Point.Position.Latitude; // current latitude
               longitude = pos.Coordinate.Point.Position.Longitude;
               GetNearbyData();
           }
           catch (Exception ex)
           {
               if ((uint)ex.HResult == 0x80004004)
               {
                   // the application does not have the right capability or the location master switch is off
                   App.MessageCustom("Location Disabled", "We are unable to determine your location");
               }
           
               {
                   latitude = 74.320855;
                   longitude = 31.514255;
                   GetNearbyData();
               }
           }
          
        }
        #endregion
        #region AZURE
        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                    items = await todoTable
                    .Where(todoItem => todoItem.isDeleted == false)
                    .ToCollectionAsync();

                    Achitems = await achivTable
                .Where(todoItem => todoItem.isDeleted == false)
                  .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                App.MessageCustom("Internet :(","Can't Connect");
            }
            else
            {
                lbx_Posts.ItemsSource = items;
                foreach (Achievements ach in Achitems) 
                {
                    if (ach.AchievementBy.Equals(App._AppUser.Id)) 
                    {
                        obs_achvments.Add(ach);
                    }
                }


                lbx_Achv.ItemsSource = obs_achvments;
            }

        }

     
#endregion
        #region LifeCycle Events
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //Selection Changed Route Detached
            //lbx_articles.SelectionChanged -= lbx_articles_SelectionChanged;
            //lbx_nearby.SelectionChanged -= lbx_nearby_SelectionChanged;
            //lbx_Posts.SelectionChanged -= lbx_Posts_SelectionChanged;

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            tbl_Stars.Text = App._AppUser.Stars + "";
            tbl_HealthPoints.Text = App._AppUser.HealthPoints + "";
            tbl_PetName.Text = App._AppUser.PetName;

            if (App.IsSessionDataAvailable == true)
            {
                tbl_avgspeed.Text = App.SessionData.AverageSpeed;
                tbl_calories.Text = App.SessionData.Calories;
                tbl_distance.Text = App.SessionData.Distance;
                tbl_pace.Text = App.SessionData.Pace;
                tbl_time.Text = App.SessionTime.ToString(@"hh\:mm\:ss");
            }
            
            //Selection Changed Route Attached
            //lbx_articles.SelectionChanged += lbx_articles_SelectionChanged;
            //lbx_nearby.SelectionChanged += lbx_nearby_SelectionChanged;
            //lbx_Posts.SelectionChanged += lbx_Posts_SelectionChanged;


            try
            {
                await RefreshTodoItems();
            }
            catch (Exception exc)
            {
            }

        }
       
        #endregion
        #region JSON
        public async void GetNearbyData() 
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("https://api.foursquare.com/v2/venues/search?client_id=4UKK0YO0NDIKPWGOELRGU5TR2PZNQXOLJ3N42KKQRUX0DXLM&client_secret=XW1NITAJESHVCB3PTDPDMXJNALMGDDI21VYMX1Z5GSQWIVBU&v=20130815%20&ll=40.7,-74&query=park");
                var result = await response.Content.ReadAsStringAsync();

                var rootObject = JsonConvert.DeserializeObject<RootParks>(result);

                foreach (Venue ven in rootObject.response.venues)
                {
                    vene nearbyVar = new vene();
                    nearbyVar.name = ven.name;
                    nearbyVar.country = ven.location.country;
                    nearbyVar.city = ven.location.city;
                    nearbyVar.phone = ven.contact.phone;
                    nearbyVar.distance = ven.location.distance;
                    nearbyVar.url = ven.url;
                    nearbyVar.lat = ven.location.lat;
                    nearbyVar.lng = ven.location.lng;
                    nearbyVar.checkinsCount = ven.stats.checkinsCount;
                    obs_NearbyParks.Add(nearbyVar);

                    
                }

                lbx_nearby.ItemsSource = obs_NearbyParks;
                //bind the observable collection here
            }
            catch (Exception exc)
            {
            }
        }
        public async void GetArticlesData()
        {

            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync("http://hello987.azurewebsites.net/art.html");
                var result = await response.Content.ReadAsStringAsync();

                var rootObject = JsonConvert.DeserializeObject<RootArticles>(result);

                foreach(Article everyItemInList in rootObject.Articles)
                {

                    obs_Articles.Add(everyItemInList);
                }

                lbx_articles.ItemsSource = obs_Articles;
                 }
            catch (Exception exc)
            {
            }
        }
        #endregion
        #region Taps
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void storeAppbarClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Store));
        }
        private void profileAppbarClicked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdateProfile));
        }
        private void LeaderBoardTapped(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LeaderBoard));
        }
        private void ScheduleExerciseTapped(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ScheduleExercise));
        }
        private void searchTapped(object sender, TappedRoutedEventArgs e)
        {
            lbx_Posts.ItemsSource = items.Where(x => x.PostTitle.Contains(tbx_search.Text));
        }
        private void profiletapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Profile));
        }
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreatePost));
        }
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(Kinec));
        }
        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
           this.Frame.Navigate(typeof(AppSettings));
        }
        #endregion

        private void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PetInteraction));
        }
    }
}
