using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class UpdateProfile : Page
    {
        public UpdateProfile()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainHub));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbx_Name.Text = App._AppUser.Name;
            tbx_Age.Text = App._AppUser.Age + "";
            tbx_Password.Text = App._AppUser.Password + "";
            tbx_Email.Text = App._AppUser.Email;
            tbx_UserName.Text = App._AppUser.UserName;
            tbx_Weight.Text = App._AppUser.Weight + "";
            tbx_Height.Text = App._AppUser.Height + "";
            tbx_PetName.Text = App._AppUser.PetName;
            tbx_Question.Text = App._AppUser.Question;
            tbx_Answer.Text = App._AppUser.Answer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Profile));
        }
    }
}
