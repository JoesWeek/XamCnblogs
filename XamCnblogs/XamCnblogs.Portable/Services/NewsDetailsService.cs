
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class NewsDetailsService : INewsDetailsService
    {
        public NewsDetailsService()
        {
        }
        public async Task<ResponseMessage> GetNewsAsync(int id)
        {
            var url = string.Format(Apis.NewsBody, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
