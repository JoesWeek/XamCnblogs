
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Net.Http;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Services
{
    public class CommentService : ICommentService
    {
        public CommentService()
        {
        }
        public async Task<ResponseMessage> PostComment(int id, string content)
        {
            var url = string.Format(Apis.StatusCommentAdd, id, content);

            content = string.Format("ReplyTo={0}&ParentCommentId={1}&Content={2}", 0, 0, content);
            return await UserHttpClient.Current.PostAsync(url,new StringContent(content));
        }
    }
}
