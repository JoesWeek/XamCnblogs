
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class KbArticlesDetailsService : IKbArticlesDetailsService
    {
        public KbArticlesDetailsService()
        {
        }
        public async Task<ResponseMessage> GetKbArticlesAsync(int id)
        {
            var url = string.Format(Apis.KbArticlesBody, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
