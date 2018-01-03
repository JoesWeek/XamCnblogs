namespace XamCnblogs.Portable.Helpers
{
    public class Apis
    {
        public const string Host = "https://api.cnblogs.com/";
        public const string Api = "api";
        public const string Token = "https://oauth.cnblogs.com/connect/token";

        public const string Authorize = "https://oauth.cnblogs.com/connect/authorize?client_id={0}&scope=openid profile CnBlogsApi offline_access&response_type=code id_token&redirect_uri=https://oauth.cnblogs.com/auth/callback&state=cnblogs.com&nonce=cnblogs.com";

        public const string Users = Api + "/Users";

        public const string BlogApp = Api + "/blogs/{0}";
        public const string BlogPosts = Api + "/blogs/{0}/posts?pageIndex={1}&pageSize={2}";

        public const string ArticleHome = Api + "/blogposts/@sitehome?pageIndex={0}&pageSize={1}";
        public const string ArticleHot = Api + "/blogposts/@picked?pageIndex={0}&pageSize={1}";
        public const string ArticleBody = Api + "/blogposts/{0}/body";
        public const string ArticleComment = Api + "/blogs/{0}/posts/{1}/comments?pageIndex={2}&pageSize={3}";
        public const string ArticleCommentAdd = Api + "/blogs/{0}/posts/{1}/comments";

        public const string News = Api + "/NewsItems?pageIndex={0}&pageSize={1}";
        public const string NewsHome = Api + "/newsitems?pageIndex={0}&pageSize={1}";
        public const string NewsRecommend = Api + "/newsitems/@recommended?pageIndex={0}&pageSize={1}";
        public const string NewsWorkHot = Api + "/newsitems/@hot-week?pageIndex={0}&pageSize={1}";
        public const string NewsBody = Api + "/newsitems/{0}/body";
        public const string NewsComment = Api + "/news/{0}/comments?pageIndex={1}&pageSize={2}";
        public const string NewsCommentAdd = Api + "/news/{0}/comments";
        public const string NewsCommentEdit = Api + "/newscomments/{0}";

        public const string KbArticles = Api + "/KbArticles?pageIndex={0}&pageSize={1}";
        public const string KbArticlesBody = Api + "/kbarticles/{0}/body";

        public const string Status = Api + "/statuses/@{0}?pageIndex={1}&pageSize={2}&tag=";
        public const string StatusBody = Api + "/statuses/{0}";
        public const string StatusADD = Api + "/statuses";
        public const string StatusDelete = Api + "/statuses/{0}";
        public const string StatusComments = Api + "/statuses/{0}/comments";
        public const string StatusCommentAdd = Api + "/statuses/{0}/comments";
        public const string StatusCommentDelete = Api + "/statuses/{0}/comments/{1}";

        public const string Questions = Api + "/questions/@sitehome?pageIndex={0}&pageSize={1}";
        public const string QuestionsType = Api + "/questions/@{0}?pageIndex={1}&pageSize={2}";
        public const string QuestionADD = Api + "/questions";
        public const string QuestionDetails = Api + "/questions/{0}";
        public const string QuestionEdit = Api + "/questions/{0}";
        public const string QuestionsAnswers = Api + "/questions/{0}/answers";
        public const string QuestionsAnswerByUser = Api + "/questions/{0}?userId={1}";
        public const string QuestionsAnswerAdd = Api + "/questions/{0}/answers";
        public const string QuestionsAnswerEdit = Api + "/questions/{0}/answers/{1}";
        public const string QuestionsAnswerComments = Api + "/questions/answers/{0}/comments";
        public const string QuestionsAnswerCommentsAdd = Api + "/questions/{0}/answers/{1}/comments";
        public const string QuestionsAnswerCommentsEdit = Api + "/questions/{0}/answers/{1}/comments/{2}";

        public const string Bookmarks = Api + "/Bookmarks?pageIndex={0}&pageSize={1}";
        public const string BookmarkHead = Api + "/Bookmarks?url={0}";
        public const string BookmarkEdit = Api + "/bookmarks/{0}";
        public const string BookmarkAdd = Api + "/Bookmarks";

        public const string Search = Host + Api + "/ZzkDocuments/{0}?keyWords={1}&pageIndex={2}&startDate=&endDate=&viewTimesAtLeast=0";

    }
}
