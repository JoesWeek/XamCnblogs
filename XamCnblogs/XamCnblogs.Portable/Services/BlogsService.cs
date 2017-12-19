
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class BlogsService : IBlogsService
    {
        public BlogsService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(string blogApp, int pageIndex = 1, int pageSize = 20)
        {
            var url = string.Format(Apis.BlogPosts, blogApp, pageIndex, pageSize);
            return await UserHttpClient.Current.GetAsyn(url);
        }
    }
}
