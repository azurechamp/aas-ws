using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class PostView : Page
    {

        ObservableCollection<Comment> obs_cmnt = new ObservableCollection<Comment>();
        private MobileServiceCollection<Comment, Comment> items;
        private IMobileServiceTable<Comment> todoTable = App.MobileService.GetTable<Comment>();
        string postId = App._PostData.Id;

        public PostView()
        {
            this.InitializeComponent();
            tbl_postTitle.Text = App._PostData.PostTitle;
            tbl_postDisc.Text = App._PostData.PostDisc;
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


        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainHub));
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
                App.MessageCustom("Error!", "Error loading data from Internet");
            }
            else
            {
                foreach (Comment cmt in items)
                {
                    if (cmt.PostNumber.Equals(postId))
                    {

                        string commentPerson = "";
                     
                        foreach (User us in App._userData) 
                        {
                            if (us.Id.Equals(cmt.CommentOf)) 
                            {
                                commentPerson = us.Name;
                            }
                        }
                        stk_comments.Children.Add(new CommentUserControl { Comment = cmt.CommentDisc , CommentBy = commentPerson});

                       
                    
                    }

                }
              

            }
        }


        private async Task InsertTodoItem(Comment todoItem)
        {

            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);


        }

        private async void btn_comment_Click(object sender, RoutedEventArgs e)
        {
            var _comment = new Comment { CommentDisc = tbx_comment.Text, PostNumber =postId, CommentOf = App._AppUser.Id };
            await InsertTodoItem(_comment);
            stk_comments.Children.Add(new CommentUserControl { Comment = _comment.CommentDisc });
            tbx_comment.Text = "";
        }



     
        
    }
}
