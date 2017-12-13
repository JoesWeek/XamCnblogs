using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IArticlesCommentService
    {
        Task<ResponseMessage> GetCommentAsync(string blogApp, int id,int pageIndex);
        Task<ResponseMessage> PostCommentAsync(string blogApp, int id, string content);
    }
}
