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
        IArticlesCommentService ArticlesCommentService { get; }
        INewsService NewsService { get; }
        INewsDetailsService NewsDetailsService { get; }
        INewsCommentService NewsCommentService { get; }
        IKbArticlesService KbArticlesService { get; }
        IKbArticlesDetailsService KbArticlesDetailsService { get; }
        IStatusesService StatusesService { get; }
        IStatusesCommentService StatusesCommentsService { get; }
        IQuestionsService QuestionsService { get; }
        IQuestionsDetailsService QuestionsDetailsService { get; }
        IQuestionsAnswersService QuestionsAnswersService { get; }
        IAnswersCommentService AnswersCommentService { get; }
        ICommentService CommentService { get; }
        Task<bool> SyncAllAsync(bool syncUserSpecific);
        Task DropEverythingAsync();
    }
}
