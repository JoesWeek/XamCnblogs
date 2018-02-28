using FormsToolkit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Controls;
using XamCnblogs.UI.Pages.Account;
using XamCnblogs.UI.Pages.Article;
using XamCnblogs.UI.Pages.New;
using XamCnblogs.UI.Pages.Question;
using XamCnblogs.UI.Pages.Status;

namespace XamCnblogs.UI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            AppCenter.Start("", typeof(Analytics), typeof(Crashes));
            ViewModelBase.Init();

            XamBottomBarPage bottomBarPage = new XamBottomBarPage() { Title = "博客园" };
            bottomBarPage.BarTextColor = (Color)Application.Current.Resources["Primary"];
            bottomBarPage.FixedMode = true;

            bottomBarPage.BarTheme = XamBottomBarPage.BarThemeTypes.Light;
            
            bottomBarPage.Children.Add(new ArticlesTopTabbedPage());
            bottomBarPage.Children.Add(new NewsTopTabbedPage());
            bottomBarPage.Children.Add(new StatusesTopTabbedPage());
            bottomBarPage.Children.Add(new QuestionsTopTabbedPage());
            bottomBarPage.Children.Add(new AccountPage());
            
            MainPage = new NavigationPage(bottomBarPage);
            
        }
        protected override void OnStart()
        {
            OnResume();
        }
        public void SecondOnResume()
        {
            OnResume();
        }
        bool registered;
        protected async override void OnResume()
        {
            await RefreshUserTokenAsync();

            if (registered)
                return;
            registered = true;

            MessagingService.Current.Subscribe(MessageKeys.NavigateLogin, async m =>
            {
                Page page = new NavigationPage(new AuthorizePage());

                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                await NavigationService.PushModalAsync(nav, page);
            });
            MessagingService.Current.Subscribe<string>(MessageKeys.NavigateToken, async (m, q) =>
            {
                var result = await TokenHttpClient.Current.PostTokenAsync(q);
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

                        var nav = Application.Current?.MainPage?.Navigation;
                        if (nav == null)
                            return;
                        await nav.PopModalAsync();
                    }
                }
            });
            MessagingService.Current.Subscribe(MessageKeys.NavigateAccount, async m =>
            {
                Page page = new NavigationPage(new AccountPage());

                var nav = Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                await NavigationService.PushModalAsync(nav, page);
            });
        }
        protected override void OnSleep()
        {
            if (!registered)
                return;

            registered = false;
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateLogin);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateToken);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateAccount);
        }

        private async Task RefreshUserTokenAsync()
        {
            if (UserTokenSettings.Current.UserRefreshToken != null)
            {
                var result = await UserHttpClient.Current.RefreshTokenAsync();
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

                    }
                }
            }
        }
    }
}