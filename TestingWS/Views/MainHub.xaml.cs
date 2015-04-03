using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using TestingApp.DataModels;
using TestingApp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestingWS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainHub : Page
    {

        private ObservableCollection<Achievements> obs_achvments = new ObservableCollection<Achievements>();
        private ObservableCollection<vene> obs_NearbyParks = new ObservableCollection<vene>();
        private ObservableCollection<Article> obs_Articles = new ObservableCollection<Article>();
        private MobileServiceCollection<Post, Post> items;
        private IMobileServiceTable<Post> todoTable = App.MobileService.GetTable<Post>();
        private MobileServiceCollection<Achievements, Achievements> Achitems;
        private IMobileServiceTable<Achievements> achivTable = App.MobileService.GetTable<Achievements>();


        double latitude, longitude;


        public MainHub()
        {
            this.InitializeComponent();
            GetArticlesData();
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
    }
}
