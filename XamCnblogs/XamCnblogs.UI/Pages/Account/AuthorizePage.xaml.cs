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
        private bool isCompleted;
        ActivityIndicatorPopupPage popupPage;
        public AuthorizePage()
        {
            InitializeComponent();
            Title = "登录";
            
            FormsWebView.OnNavigationCompleted += OnNavigationCompleted;
            FormsWebView.OnNavigationError += OnNavigationError;

            FormsWebView.Source = string.Format(Apis.Authorize, TokenHttpClient.ClientId);

            popupPage = new ActivityIndicatorPopupPage();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!isCompleted)
            {
                Navigation.PushPopupAsync(popupPage);
            }
        }
        
        private async void OnNavigationError(object sender, int e)
        {
            await Navigation.RemovePopupPageAsync(popupPage);
        }

        private async void OnNavigationCompleted(object sender, string url)
        {
            isCompleted = false;
            if (url.IndexOf("https://passport.cnblogs.com/user/signin?returnUrl=") > -1)
            {
                isCompleted = true;
                await Navigation.RemovePopupPageAsync(popupPage);
            }
            if (url.IndexOf("https://oauth.cnblogs.com/auth/callback#code=") > -1)
            {
                if (popupPage == null)
                {
                    popupPage = new ActivityIndicatorPopupPage();
                }
                await Navigation.PushPopupAsync(popupPage);
                try
                {
                    FormsWebView.IsVisible = false;
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

                                isCompleted = true;
                                await Navigation.RemovePopupPageAsync(popupPage);
                                await Navigation.PopAsync();
                            }
                            else
                            {
                                await DisplayAlert("登录", "登录失败", "确定");
                            }
                        }
                        else
                        {
                            await DisplayAlert("登录", "获取token失败", "确定");
                        }
                    }
                    else
                    {
                        await DisplayAlert("登录", "获取Code失败", "确定");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("登录", ex.Message, "确定");
                }
            }
        }


        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            isCompleted = true;
            await Navigation.RemovePopupPageAsync(popupPage);
        }
    }
}