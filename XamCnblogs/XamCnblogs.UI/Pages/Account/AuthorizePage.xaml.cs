using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class AuthorizePage : ContentPage
    {
        public AuthorizePage()
        {
            InitializeComponent();
            Title = "登录";

            var cancel = new ToolbarItem
            {
                Text = "关闭",
                Command = new Command(async () =>
                {
                    await Navigation.PopModalAsync();
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_close.png";

            authorizeView.Source = string.Format(Apis.Authorize, TokenHttpClient.ClientId);

            authorizeView.AuthorizeStarted += async (sender, e) =>
            {
                activityIndicator.IsRunning = true;

                var result = await TokenHttpClient.Current.PostTokenAsync(e.Code);
                if (result.Success)
                {
                    var token = JsonConvert.DeserializeObject<Token>(result.Message.ToString());
                    token.RefreshTime = DateTime.Now;
                    UserTokenSettings.Current.UpdateUserToken(token);
                    var userResult = await UserHttpClient.Current.GetAsyn(Apis.Users);
                    if (userResult.Success)
                    {
                        var user = JsonConvert.DeserializeObject<User>(userResult.Message.ToString());
                        UserSettings.Current.UpdateUser(user);

                        activityIndicator.IsRunning = false;

                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        activityIndicator.IsRunning = false;
                        await DisplayAlert("登录", "登录失败", "确定");
                    }
                }
                else
                {
                    activityIndicator.IsRunning = false;
                    await DisplayAlert("登录", "获取token失败", "确定");
                }
            };

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            activityIndicator.IsRunning = false;
        }
    }
}