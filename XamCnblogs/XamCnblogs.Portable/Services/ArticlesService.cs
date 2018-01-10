
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCnblogs.Portable.Services
{
    public class ArticlesService : IArticlesService
    {
        public ArticlesService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(int position, int pageIndex = 1, int pageSize = 20)
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
            try
            {
                return await TokenHttpClient.Current.GetAsyn(url);
            }
            catch (System.Exception ex)
            {
                var result = new ResponseMessage();
                result.Success = false;
                result.Message = ex.Message;
                DependencyService.Get<ILog>().SendLog("ArticlesService.GetArticlesAsync:" + ex.Message);
                return result;
            }
        }
    }
}
