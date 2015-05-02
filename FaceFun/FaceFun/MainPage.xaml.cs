using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace FaceFun
{
   
    public sealed partial class MainPage : Page
    {


        private string APIKey = "86159233d1b24923bd31beac39338ed2";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            img_selectedImage.Source = new BitmapImage(new Uri(Url.Text));
            MakeRequest(Url.Text);
        }

       
       
       private async void MakeRequest( string UrlofImage)
        {
                var client = new HttpClient();
                var uri = "https://api.projectoxford.ai/face/v0/detections?subscription-key="+APIKey+"&analyzesFaceLandmarks=true&analyzesAge=true&analyzesGender=true&analyzesHeadPose=true";
                HttpResponseMessage response;
                var Content = new StringContent("{\'url\':\'"+UrlofImage+"\'}", Encoding.UTF8, "application/json");
                response = await client.PostAsync(uri,Content);

                if (response.Content != null)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                textBlock.Text = responseString;

            }
           
        }


       
    }
}
