
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class NewsCommentService : INewsCommentService
    {
        int pageSize = 10;
        public NewsCommentService()
        {
        }
        public async Task<ResponseMessage> GetCommentAsync(int id, int pageIndex)
        {
            var url = string.Format(Apis.NewsComment, id, pageIndex, pageSize);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
