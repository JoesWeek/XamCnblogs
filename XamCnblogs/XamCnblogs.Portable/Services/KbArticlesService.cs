
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class KbArticlesService : IKbArticlesService
    {
        public KbArticlesService()
        {
        }
        public async Task<ResponseMessage> GetKbArticlesAsync(int pageIndex = 1, int pageSize = 20)
        {
            var url = string.Format(Apis.KbArticles, pageIndex, pageSize);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
