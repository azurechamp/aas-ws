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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TestingWS
{
    public sealed partial class CommentUserControl : UserControl
    {
        public CommentUserControl()
        {
            this.InitializeComponent();
        }



        public string CommentBy
        {
            get { return (string)GetValue(CommentByProperty); }
            set { SetValue(CommentByProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommentBy.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentByProperty =
            DependencyProperty.Register("CommentBy", typeof(string), typeof(CommentUserControl), null);

        

        public string Comment
        {
            get { return (string)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Comment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentProperty =
            DependencyProperty.Register("Comment", typeof(string), typeof(CommentUserControl), null);

        
    }
}
