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
}