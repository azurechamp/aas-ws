using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestingApp.DataModels;
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

namespace TestingWS.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Profile : Page
    {

        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
      
        public Profile()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UpdateProfile));
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
                MessageCustom("Internet Problem","There was problem in Internet Connection");
            }
            else
            {
                btn_signUp.IsEnabled = true;
            }


        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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

            await RefreshTodoItems();


        }

        private async void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            var usr = new User { Email = tbx_Email.Text, Password = tbx_Password.Text, UserName = tbx_UserName.Text, Age = float.Parse(tbx_Age.Text, CultureInfo.InvariantCulture.NumberFormat), Height = float.Parse(tbx_Height.Text, CultureInfo.InvariantCulture.NumberFormat), Weight = float.Parse(tbx_Weight.Text, CultureInfo.InvariantCulture.NumberFormat), PetName = tbx_PetName.Text, Question = tbx_Question.Text, Answer = tbx_Answer.Text, HealthPoints = 100f, Name = tbx_Name.Text, Stars = 250f, Id = App._AppUser.Id };
            await UpdateCheckedTodoItem(usr);
            MessageCustom("UPDATED!","Your Profile has been Updated");
        }
        private async Task UpdateCheckedTodoItem(User item)
        {
            await todoTable.UpdateAsync(item);
            items.Remove(item);


            //await SyncAsync(); // offline sync
        }

        public async void MessageCustom(string title, string details)
        {

            await new MessageDialog(details, title).ShowAsync();
        }

    }
}
