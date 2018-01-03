using System.Threading.Tasks;
using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;

namespace XamCnblogs.Portable.Services
{
    public class StoreManager : IStoreManager
    {
        #region IStoreManager implementation

        public Task<bool> SyncAllAsync(bool syncUserSpecific)
        {
            return Task.FromResult(true);
        }

        public bool IsInitialized { get { return true; } }
        public Task InitializeAsync()
        {
            return Task.FromResult(true);
        }

        #endregion

        public Task DropEverythingAsync()
        {
            return Task.FromResult(true);
        }

        IArticlesService articlesService;
        public IArticlesService ArticlesService => articlesService ?? (articlesService = DependencyService.Get<IArticlesService>());

        IArticlesDetailsService articlesDetailsService;
        public IArticlesDetailsService ArticlesDetailsService => articlesDetailsService ?? (articlesDetailsService = DependencyService.Get<IArticlesDetailsService>());
        

        INewsService newsService;
        public INewsService NewsService => newsService ?? (newsService = DependencyService.Get<INewsService>());

        INewsDetailsService newsDetailsService;
        public INewsDetailsService NewsDetailsService => newsDetailsService ?? (newsDetailsService = DependencyService.Get<INewsDetailsService>());
        
        IKbArticlesService kbarticlesService;
        public IKbArticlesService KbArticlesService => kbarticlesService ?? (kbarticlesService = DependencyService.Get<IKbArticlesService>());

        IKbArticlesDetailsService kbArticlesDetailsService;
        public IKbArticlesDetailsService KbArticlesDetailsService => kbArticlesDetailsService ?? (kbArticlesDetailsService = DependencyService.Get<IKbArticlesDetailsService>());

        IStatusesService statusesService;
        public IStatusesService StatusesService => statusesService ?? (statusesService = DependencyService.Get<IStatusesService>());

        IStatusesCommentService statusesCommentsService;
        public IStatusesCommentService StatusesCommentsService => statusesCommentsService ?? (statusesCommentsService = DependencyService.Get<IStatusesCommentService>());

        IQuestionsService questionsService;
        public IQuestionsService QuestionsService => questionsService ?? (questionsService = DependencyService.Get<IQuestionsService>());

        IQuestionsDetailsService questionsDetailsService;
        public IQuestionsDetailsService QuestionsDetailsService => questionsDetailsService ?? (questionsDetailsService = DependencyService.Get<IQuestionsDetailsService>());

        IAnswersDetailsService answersCommentService;
        public IAnswersDetailsService AnswersDetailsService => answersCommentService ?? (answersCommentService = DependencyService.Get<IAnswersDetailsService>());
        
        IBlogsService blogsService;
        public IBlogsService BlogsService => blogsService ?? (blogsService = DependencyService.Get<IBlogsService>());

        IBookmarksService bookmarksService;
        public IBookmarksService BookmarksService => bookmarksService ?? (bookmarksService = DependencyService.Get<IBookmarksService>());

        ISearchService searchService;
        public ISearchService SearchService => searchService ?? (searchService = DependencyService.Get<ISearchService>());

    }
}
