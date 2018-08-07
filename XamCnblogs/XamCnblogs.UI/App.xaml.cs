using FormsToolkit;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Controls;
using XamCnblogs.UI.Pages.Account;
using XamCnblogs.UI.Pages.Article;
using XamCnblogs.UI.Pages.KbArticle;
using XamCnblogs.UI.Pages.New;
using XamCnblogs.UI.Pages.Question;
using XamCnblogs.UI.Pages.Search;
using XamCnblogs.UI.Pages.Status;

namespace XamCnblogs.UI
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            VersionTracking.Track();

            SqliteUtil.Current.CreateAllTablesAsync();

            AppCenter.Start("", typeof(Analytics), typeof(Crashes));

            ViewModelBase.Init();

            var bottomBarPage = new HomeTabbedPage() { Title = "博客园" };
            bottomBarPage.BackgroundColor = Color.White;
            bottomBarPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom).SetOffscreenPageLimit(5).SetElevation(5F);
            bottomBarPage.Children.Add(new ArticlesTopTabbedPage());
            bottomBarPage.Children.Add(new NewsTopTabbedPage());
            bottomBarPage.Children.Add(new StatusesTopTabbedPage());
            bottomBarPage.Children.Add(new QuestionsTopTabbedPage());
            bottomBarPage.Children.Add(new AccountPage());

            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                var rootPage = new Pages.Android.RootPage();
                rootPage.Children.Add(bottomBarPage);

                rootPage.Children.Add(new SearchPage());

                MainPage = new XamNavigationPage(rootPage);
            }
            else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            {
                MainPage = new XamNavigationPage(bottomBarPage);
            }
        }

        protected override void OnStart()
        {
            OnResume();
        }
        bool registered;
        protected async override void OnResume()
        {
            await UserTokenSettings.Current.RefreshUserTokenAsync();

            if (registered)
                return;
            registered = true;
            Connectivity.ConnectivityChanged += ConnectivityChanged;

            MessagingService.Current.Subscribe(MessageKeys.NavigateLogin, async m =>
            {
                Page page = new NavigationPage(new AuthorizePage());

                var nav = Xamarin.Forms.Application.Current?.MainPage?.Navigation;
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

                        var nav = Xamarin.Forms.Application.Current?.MainPage?.Navigation;
                        if (nav == null)
                            return;
                        await nav.PopModalAsync();
                    }
                }
            });
            MessagingService.Current.Subscribe(MessageKeys.NavigateAccount, async m =>
            {
                Page page = new NavigationPage(new AccountPage());

                var nav = Xamarin.Forms.Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;

                await NavigationService.PushModalAsync(nav, page);
            });
            MessagingService.Current.Subscribe<string>(MessageKeys.NavigateNotification, async (m, message) =>
            {
                DependencyService.Get<IToast>().SendToast(message);
                var nav = Xamarin.Forms.Application.Current?.MainPage?.Navigation;
                if (nav == null)
                    return;
                Page page = null;
                var notification = JsonConvert.DeserializeObject<Notification>(message);
                if (notification != null)
                {
                    switch (notification.Type)
                    {
                        case "articles":
                            page = new NavigationPage(new ArticlesDetailsPage(new Articles() { Title = notification.Title, Id = notification.ID }));
                            break;
                        case "news":
                            page = new NavigationPage(new NewsDetailsPage(new News() { Title = notification.Title, Id = notification.ID }));
                            break;
                        case "kbarticles":
                            page = new NavigationPage(new KbArticlesDetailsPage(new KbArticles() { Title = notification.Title, Id = notification.ID }));
                            break;
                        case "questions":
                            page = new NavigationPage(new QuestionsDetailsPage(new Questions() { Title = notification.Title, Qid = notification.ID }));
                            break;
                        case "update":
                            if (notification.ID > int.Parse(VersionTracking.CurrentBuild))
                            {
                                if (await Xamarin.Forms.Application.Current?.MainPage.DisplayAlert("新版提示", notification.Title, "立即下载", "取消"))
                                {
                                    await ViewModelBase.ExecuteLaunchBrowserAsync(notification.Url);
                                }
                            }
                            return;
                        default:
                            return;
                    }
                }
                await NavigationService.PushAsync(nav, page);
            });
        }

        private async void ConnectivityChanged(Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                DependencyService.Get<IToast>().SendToast("网络不给你，请检查网络设置");
            }
            else
            {
                await UserTokenSettings.Current.RefreshUserTokenAsync();
            }
        }

        protected override void OnSleep()
        {
            if (!registered)
                return;

            registered = false;
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateLogin);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateToken);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateAccount);
            MessagingService.Current.Unsubscribe(MessageKeys.NavigateNotification);

            Connectivity.ConnectivityChanged -= ConnectivityChanged;
        }

    }
}