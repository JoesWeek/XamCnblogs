
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class ArticlesDetailsService : IArticlesDetailsService
    {
        public ArticlesDetailsService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(int id)
        {
            var url = string.Format(Apis.ArticleBody, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
