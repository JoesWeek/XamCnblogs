using XamCnblogs.Portable.Interfaces;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IStoreManager
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        IArticlesService ArticlesService { get; }
        IArticlesDetailsService ArticlesDetailsService { get; }
        INewsService NewsService { get; }
        INewsDetailsService NewsDetailsService { get; }
        IKbArticlesService KbArticlesService { get; }
        IKbArticlesDetailsService KbArticlesDetailsService { get; }
        IStatusesService StatusesService { get; }
        IStatusesCommentService StatusesCommentsService { get; }
        IQuestionsService QuestionsService { get; }
        IQuestionsDetailsService QuestionsDetailsService { get; }
        IAnswersDetailsService AnswersDetailsService { get; }
        IBlogsService BlogsService { get; }
        IBookmarksService BookmarksService { get; }
        ISearchService SearchService { get; }
        Task<bool> SyncAllAsync(bool syncUserSpecific);
        Task DropEverythingAsync();
    }
}
