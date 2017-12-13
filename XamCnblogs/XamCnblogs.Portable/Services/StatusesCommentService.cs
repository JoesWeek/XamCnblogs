
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Services
{
    public class StatusesCommentService : IStatusesCommentService
    {
        public StatusesCommentService()
        {
        }
        public async Task<ResponseMessage> GetStatusesCommentsAsync(int id)
        {
            var url = string.Format(Apis.StatusComments, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
