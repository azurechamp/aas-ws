using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestingApp.DataModels;
using TestingWS.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestingWS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
     

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshTodoItems();
        }

        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems
                items = await todoTable
                    .Where(todoItem => todoItem.isDeleted == false)
                    .ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
               MessageCustom("Gymnasio and Internet are unable to connect", "Connection");
            }
            else
            {
                btn_signIn.IsEnabled = true;
                App._userData = items;
            }

        }

        bool _userExist;
        private void btn_signIn_Click(object sender, RoutedEventArgs e)
        {
            foreach (User usr in items)
            {
                if (usr.UserName.Equals(tbx_UserName.Text.Trim()) && usr.Password.Equals(tbx_Password.Password.Trim()))
                {
                    _userExist = true;
                    App._AppUser = usr;
                }
            }

            if (_userExist == true)
            {

                this.Frame.Navigate(typeof(MainHub));
                
                _userExist = false;
            }
            else
            {
                MessageCustom("Whoops!!", "User with this username and password \ndoes not exists");
                tbx_Password.Password = "";
                tbx_UserName.Text = "";

            }
        }

        public async void MessageCustom(string title, string details)
        {

            await new MessageDialog(details, title).ShowAsync();
        }
    }
}
