using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.UI.Pages.About
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

            WeibaSwitchToggled(AboutSettings.Current.WeibaToggled);

            WeibaSwitch.Toggled += (object sender, ToggledEventArgs e) =>
            {
                WeibaSwitchToggled(e.Value);
            };
        }
        void WeibaSwitchToggled(bool toggled)
        {
            if (toggled)
            {
                WeibaButton.TextColor = (Color)Application.Current.Resources["PrimaryText"];
            }
            else
            {
                WeibaButton.TextColor = (Color)Application.Current.Resources["SecondaryText"];
            }
            AboutSettings.Current.WeibaToggled = WeibaSwitch.IsToggled = toggled;
        }
        async void OnSettingWeiba(object sender, EventArgs args)
        {
            if (WeibaSwitch.IsToggled)
            {
                await NavigationService.PushAsync(Navigation, new SettingWeibaPage());
            }
        }
    }
}