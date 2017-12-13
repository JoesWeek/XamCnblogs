
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class ArticlesService : IArticlesService
    {
        private int pageSize = 10;
        public ArticlesService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(int position, int pageIndex = 1)
        {
            var url = "";
            switch (position)
            {
                case 1:
                    url = string.Format(Apis.ArticleHot, pageIndex, pageSize);
                    break;
                default:
                    url = string.Format(Apis.ArticleHome, pageIndex, pageSize);
                    break;
            }
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
