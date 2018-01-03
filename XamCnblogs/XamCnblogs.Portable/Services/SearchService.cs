
using System.Threading.Tasks;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;


namespace XamCnblogs.Portable.Services
{
    public class SearchService : ISearchService
    {
        public SearchService()
        {
        }
        public async Task<ResponseMessage> GetSearchAsync(int position, string keyWords, int pageIndex = 1, int pageSize = 20)
        {
            var url = "";
            switch (position)
            {
                case 0:
                    url = string.Format(Apis.Search, "bolgs", keyWords, pageIndex, pageSize);
                    break;
                case 1:
                    url = string.Format(Apis.Search, "news", keyWords, pageIndex, pageSize);
                    break;
                case 2:
                    url = string.Format(Apis.Search, "kb", keyWords, pageIndex, pageSize);
                    break;
                case 3:
                    url = string.Format(Apis.Search, "question", keyWords, pageIndex, pageSize);
                    break;
            }
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
