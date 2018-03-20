
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class NewsService : INewsService
    {
        public NewsService()
        {
        }
        public async Task<ResponseMessage> GetNewsAsync(int position, int pageIndex = 1, int pageSize = 20)
        {
            var url = "";
            switch (position)
            {
                case 1:
                    url = string.Format(Apis.NewsRecommend, pageIndex, pageSize);
                    break;
                case 2:
                    url = string.Format(Apis.NewsWorkHot, pageIndex, pageSize);
                    break;
                default:
                    url = string.Format(Apis.NewsHome, pageIndex, pageSize);
                    break;
            }
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
