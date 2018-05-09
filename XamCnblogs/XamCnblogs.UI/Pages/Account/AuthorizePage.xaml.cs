using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Diagnostics;
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

            formsWebView.Source = string.Format(Apis.Authorize, TokenHttpClient.ClientId);
            
            formsWebView.OnNavigationCompleted += async delegate (object sender, string url)
            {
                stackLayout.IsVisible = false;
                activityIndicator.IsRunning = false;
                if (url.IndexOf("https://oauth.cnblogs.com/auth/callback#code=") > -1)
                {
                    formsWebView.IsVisible = false;

                    stackLayout.IsVisible = true;
                    activityIndicator.IsRunning = true;

                    var codeindex = url.IndexOf("#code=") + 6;
                    var tokenindex = url.IndexOf("&id_token=");
                    var code = url.Substring(codeindex, tokenindex - codeindex);
                    if (code != "")
                    {
                        var result = await TokenHttpClient.Current.PostTokenAsync(code);
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

                                stackLayout.IsVisible = false;
                                activityIndicator.IsRunning = false;

                                await Navigation.PopModalAsync();
                            }
                            else
                            {
                                stackLayout.IsVisible = false;
                                activityIndicator.IsRunning = false;
                                await DisplayAlert("登录", "登录失败", "确定");
                            }
                        }
                        else
                        {
                            stackLayout.IsVisible = false;
                            activityIndicator.IsRunning = false;
                            await DisplayAlert("登录", "获取token失败", "确定");
                        }
                    }
                }
            };

            stackLayout.IsVisible = true;
            activityIndicator.IsRunning = true;
        }

    }
}