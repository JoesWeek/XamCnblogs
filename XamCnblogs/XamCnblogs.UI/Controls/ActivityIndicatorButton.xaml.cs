using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamCnblogs.UI.Controls
{
    public partial class ActivityIndicatorButton : ContentView
    {
        public ActivityIndicatorButton()
        {
            InitializeComponent();
            this.PropertyChanged += ActivityIndicatorButton_PropertyChanged;
            var sen = this.FindByName<StackLayout>("StackLayout1");
        }

        private void ActivityIndicatorButton_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "WidthRequest")
            {
                Indicator.WidthRequest = WidthRequest - 10;
            }
            if (e.PropertyName == "HeightRequest")
            {
                Indicator.HeightRequest = HeightRequest - 10;
            }
        }

        void OnSend(object sender, EventArgs args)
        {
            ActivityIndicatorClick?.Invoke(this, new ActivityIndicatorClickEventArgs() { ActionID = ActionID });
        }
        public event ActivityIndicatorClickEventHandler ActivityIndicatorClick;

        public static readonly BindableProperty ActionIDProperty = BindableProperty.Create(nameof(ActionID), typeof(int), typeof(ActivityIndicatorButton), 0);
        public int ActionID
        {
            get { return (int)GetValue(ActionIDProperty); }
            set { SetValue(ActionIDProperty, value); }
        }
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(ActivityIndicatorButton), "", propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
        {
            var vib = bindable as ActivityIndicatorButton;
            var send = vib.Content.FindByName<Image>("Send");
            send.Source = newValue.ToString();
        });
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public static readonly BindableProperty IsRunningProperty = BindableProperty.Create(nameof(IsRunning), typeof(bool), typeof(ActivityIndicatorButton), false, propertyChanged: (BindableObject bindable, object oldValue, object newValue) =>
        {
            var vib = bindable as ActivityIndicatorButton;
           var send = vib.Content.FindByName<Image>("Send");
            var indicator = vib.Content.FindByName<ActivityIndicator>("Indicator");
            if ((bool)newValue)
            {
                indicator.IsRunning = true;
                indicator.IsVisible = true;
                send.IsVisible = false;
            }
            else
            {
                indicator.IsRunning = false;
                indicator.IsVisible = false;
                send.IsVisible = true;
            }
        });
        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }
    }

    public class ActivityIndicatorClickEventArgs : EventArgs
    {
        public int ActionID { get; set; }
    }
    public delegate void ActivityIndicatorClickEventHandler(object sender, ActivityIndicatorClickEventArgs e);

}