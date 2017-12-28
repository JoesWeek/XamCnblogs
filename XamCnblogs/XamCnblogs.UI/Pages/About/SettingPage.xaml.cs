using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;

namespace XamCnblogs.UI.Pages.About
{
    public partial class SettingPage : ContentPage
    {
        public SettingPage()
        {
            InitializeComponent();

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares("https://github.com/JoesWeek/XamCnblogs", "博客园第三方客户端，Xamarin.Forms App，支持IOS，Android");
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";

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