using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TestingApp.DataModels;
using TestingApp.Models;
using TestingApp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class Store : Page
    {

        private MobileServiceCollection<User, User> items;
        private IMobileServiceTable<User> todoTable = App.MobileService.GetTable<User>();
        private Item selectedItem;



        public Store()
        {
            this.InitializeComponent();
            this.DataContext = new StorePageView_Model();
          
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
               MessageCustom("Error!","Check your Internet");
            }
            else
            {
                btn_buy.IsEnabled = true;

            }


        }
        void lbx_drinks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbl_HealthPoints_Copy.Text = (lbx_drinks.SelectedItem as Item).health;
            tbl_Stars_Copy.Text = (lbx_drinks.SelectedItem as Item).stars;
            tbl_prodtitle.Text = (lbx_drinks.SelectedItem as Item).name;
            img_prod.Source = new BitmapImage(new Uri((lbx_drinks.SelectedItem as Item).image));
            selectedItem = lbx_drinks.SelectedItem as Item;
            BuyItemPopUp.IsOpen = true;
        }


        void lbx_store_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbl_HealthPoints_Copy.Text = (lbx_store.SelectedItem as Item).health;
            tbl_Stars_Copy.Text = (lbx_store.SelectedItem as Item).stars;
            tbl_prodtitle.Text = (lbx_store.SelectedItem as Item).name;
            img_prod.Source = new BitmapImage(new Uri((lbx_store.SelectedItem as Item).image));
            selectedItem = lbx_store.SelectedItem as Item;
            BuyItemPopUp.IsOpen = true;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            tbl_Stars.Text = App._AppUser.Stars + "";
            tbl_HealthPoints.Text = App._AppUser.HealthPoints + "";
            lbx_store.SelectionChanged += lbx_store_SelectionChanged;
            lbx_drinks.SelectionChanged += lbx_drinks_SelectionChanged;
           
            
            try
            {
                await RefreshTodoItems();
            }
            catch (Exception exc)
            {

            }
        }

        private async Task UpdateCheckedTodoItem(User item)
        {
            await todoTable.UpdateAsync(item);
            items.Remove(item);


            //await SyncAsync(); // offline sync
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            lbx_store.SelectionChanged -= lbx_store_SelectionChanged;
            lbx_drinks.SelectionChanged -= lbx_drinks_SelectionChanged;

        }


        private async void buyItem(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            float finalStars, finalHealth;
            if (App._AppUser.Stars > float.Parse(selectedItem.stars))
            {
                finalStars = App._AppUser.Stars - float.Parse(selectedItem.stars);
                finalHealth = App._AppUser.HealthPoints + float.Parse(selectedItem.health);
                var usr = new User { Email = App._AppUser.Email, Password = App._AppUser.Password, UserName = App._AppUser.UserName, Age = App._AppUser.Age, Height = App._AppUser.Height, Weight = App._AppUser.Weight, PetName = App._AppUser.PetName, Question = App._AppUser.Question, Answer = App._AppUser.Answer, HealthPoints = finalHealth, Name = App._AppUser.Name, Stars = finalStars, Id = App._AppUser.Id };
                await UpdateCheckedTodoItem(usr);
                MessageCustom("Yepiee", "You Have bought the item");
                App._AppUser = usr;
                BuyItemPopUp.IsOpen = false;

            }
            else
            {
                BuyItemPopUp.IsOpen = false;

                MessageCustom("Opssss!!", "More Workout Needed to buy it!");
            }

        }
        
        public async void MessageCustom(string title, string details)
        {

            await new MessageDialog(details, title).ShowAsync();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainHub));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BuyItemPopUp.IsOpen = false;
        }
    }
}
