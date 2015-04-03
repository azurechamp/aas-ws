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


namespace TestingWS.Views
{
    
    public sealed partial class ScheduleExercise : Page
    {
        public ScheduleExercise()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
               var appointment = new Windows.ApplicationModel.Appointments.Appointment();

               appointment.Subject = SubjectTextBox.Text;
               appointment.StartTime = StartTimeDatePicker.Date;
               appointment.Details = DetailsTextBox.Text;

               if (AllDayCheckBox.IsChecked == true)
               {
                   appointment.AllDay = true;
               }
               else 
               {
                 appointment.AllDay =false ;
               }

               appointment.Sensitivity = Windows.ApplicationModel.Appointments.AppointmentSensitivity.Public;
             

            var rect = GetElementRect(sender as FrameworkElement);

         String appointmentId = await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Windows.UI.Popups.Placement.Default);
            if (appointmentId != String.Empty)
            {
                App.MessageCustom("HURRAH!", "Your Appointment has been added");
            }
            else
            {
                App.MessageCustom("Opps!", "We could not add appointment :( ");
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SubjectTextBox.Text = "Exercise TIME - Gymnasio";
        }
        private Windows.Foundation.Rect GetElementRect(FrameworkElement element)
        {
            Windows.UI.Xaml.Media.GeneralTransform buttonTransform = element.TransformToVisual(null);
            Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());
            return new Windows.Foundation.Rect(point, new Windows.Foundation.Size(element.ActualWidth, element.ActualHeight));
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainHub));
        }
       
    }
}
