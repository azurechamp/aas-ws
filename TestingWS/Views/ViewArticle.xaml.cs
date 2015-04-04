using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestingWS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewArticle : Page
    {

        string link = "";
        string title = "";
        string disc = "";
        public ViewArticle()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbl_Article.Text = App._SelectedArticle.title;
            tbl_disc.Text = App._SelectedArticle.disc;
            img_articleTitle.Source = new BitmapImage(new Uri(App._SelectedArticle.image));
            link = App._SelectedArticle.link;
            title = App._SelectedArticle.title;
            disc = App._SelectedArticle.disc;

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainHub));
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //Web
            try
            {
                string uriToLaunch = link;
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

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string uriToLaunch = link;
                var uri = new Uri("mailto:?to=recipient@example.com&subject="+"Gymnasio |"+title+"&body="+disc+"\n\n\n\nSent Using Gymnasio");

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

       

       


       
    }
}
