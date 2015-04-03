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
    public sealed partial class SignUp : Page
    {

        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
      
        public SignUp()
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
                App.MessageCustom("Internet Problem", "Please check your internet connection");
            }
            else
            {
                btn_signUp.IsEnabled = true;
            }

        }
        private async Task InsertTodoItem(User todoItem)
        {

            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);


            App.MessageCustom("Success", "You Have Successfully Signed Up");
            App._SignUpUser = todoItem;
            this.Frame.Navigate(typeof(MainPage));


        }
        private async void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            var usr = new User { Email = tbx_Email.Text, Password = tbx_Password.Password, UserName = tbx_UserName.Text, Age = float.Parse(tbx_Age.Text, CultureInfo.InvariantCulture.NumberFormat), Height = float.Parse(tbx_Height.Text, CultureInfo.InvariantCulture.NumberFormat), Weight = float.Parse(tbx_Weight.Text, CultureInfo.InvariantCulture.NumberFormat), PetName = tbx_PetName.Text, Question = tbx_Question.Text, Answer = tbx_Answer.Text, HealthPoints = 100f, Name = tbx_Name.Text, Stars = 250f };
            await InsertTodoItem(usr);

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
