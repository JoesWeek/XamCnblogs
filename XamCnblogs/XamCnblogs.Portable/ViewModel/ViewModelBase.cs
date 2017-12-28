using MvvmHelpers;
using Plugin.Share;
using Plugin.Share.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Services;

namespace XamCnblogs.Portable.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {
        protected INavigation Navigation { get; }

        public ViewModelBase(INavigation navigation = null)
        {
            Navigation = navigation;
        }

        public static void Init()
        {
            DependencyService.Register<IStoreManager, StoreManager>();
            DependencyService.Register<IArticlesService, ArticlesService>();
            DependencyService.Register<IArticlesDetailsService, ArticlesDetailsService>();
            DependencyService.Register<INewsService, NewsService>();
            DependencyService.Register<INewsDetailsService, NewsDetailsService>();
            DependencyService.Register<IKbArticlesService, KbArticlesService>();
            DependencyService.Register<IKbArticlesDetailsService, KbArticlesDetailsService>();
            DependencyService.Register<IStatusesService, StatusesService>();
            DependencyService.Register<IStatusesCommentService, StatusesCommentService>();
            DependencyService.Register<IQuestionsService, QuestionsService>();
            DependencyService.Register<IQuestionsDetailsService, QuestionsDetailsService>();
            DependencyService.Register<IAnswersDetailsService, AnswersDetailsService>();
            DependencyService.Register<IBlogsService, BlogsService>();
            DependencyService.Register<IBookmarksService, BookmarksService>();
        }

        public AccessTokenSettings Settings
        {
            get { return AccessTokenSettings.Current; }
        }

        protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();
        protected IToast Toast { get; } = DependencyService.Get<IToast>();
        protected ILog Log { get; } = DependencyService.Get<ILog>();
        protected IShares Shares { get; } = DependencyService.Get<IShares>();

        ICommand launchBrowserCommand;
        public ICommand LaunchBrowserCommand =>
        launchBrowserCommand ?? (launchBrowserCommand = new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));

        async Task ExecuteLaunchBrowserAsync(string arg)
        {
            if (IsBusy)
                return;

            if (!arg.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !arg.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                arg = "http://" + arg;

            var lower = arg.ToLowerInvariant();

            try
            {
                await CrossShare.Current.OpenBrowser(arg, new BrowserOptions
                {
                    ChromeShowTitle = true,
                    ChromeToolbarColor = new ShareColor
                    {
                        A = 255,
                        R = 118,
                        G = 53,
                        B = 235
                    },
                    UseSafariReaderMode = true,
                    UseSafariWebViewController = true
                });
            }
            catch (Exception ex)
            {
                Log.SendLog("LaunchBrowserCommand:" + ex.Message);
            }
        }

    }
}
