using MvvmHelpers;
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
            DependencyService.Register<IArticlesCommentService, ArticlesCommentService>();
            DependencyService.Register<INewsService, NewsService>();
            DependencyService.Register<INewsDetailsService, NewsDetailsService>();
            DependencyService.Register<INewsCommentService, NewsCommentService>();
            DependencyService.Register<IKbArticlesService, KbArticlesService>();
            DependencyService.Register<IKbArticlesDetailsService, KbArticlesDetailsService>();
            DependencyService.Register<IStatusesService, StatusesService>();
            DependencyService.Register<IStatusesCommentService, StatusesCommentService>();
            DependencyService.Register<IQuestionsService, QuestionsService>();
            DependencyService.Register<IQuestionsDetailsService, QuestionsDetailsService>();
            DependencyService.Register<IQuestionsAnswersService, QuestionsAnswersService>();
            DependencyService.Register<IAnswersCommentService, AnswersCommentService>();
            DependencyService.Register<ICommentService, CommentService>();
        }

        public AccessTokenSettings Settings
        {
            get { return AccessTokenSettings.Current; }
        }

        protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();
        protected IToast Toast { get; } = DependencyService.Get<IToast>();
    }
}
