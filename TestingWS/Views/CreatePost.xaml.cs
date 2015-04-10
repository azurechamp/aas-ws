using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
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
    public sealed partial class CreatePost : Page
    {
        private MobileServiceCollection<Post, Post> items;
        private IMobileServiceTable<Post> todoTable = App.MobileService.GetTable<Post>();
     

        public CreatePost()
        {
            this.InitializeComponent();
        }

         protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                await RefreshTodoItems();
            }
            catch (Exception exc)
            {

            }
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
                App.MessageCustom("Error!","There was Error loading the data");
            }
            else
            {
                btn_createPost.IsEnabled = true;
            }

        }

        private async Task InsertTodoItem(Post todoItem)
        {

            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);

          App.MessageCustom("Success","Post has been created successfully !");


        }

        private async void btn_createPost_Click(object sender, RoutedEventArgs e)
        {
            var _post = new Post { PostBy = App._AppUser.Id, PostTitle = tbl_title.Text, PostDisc = tbl_disc.Text };
            await InsertTodoItem(_post);
        }
    

        private void  AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainHub));
        }
    }
}
