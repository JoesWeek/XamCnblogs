using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IStatusesCommentService
    {
        Task<ResponseMessage> GetCommentsAsync(int id);
        Task<ResponseMessage> PostCommentAsync(int id, string content);
        Task<ResponseMessage> DeleteCommentAsync(int statusId, int id);
    }
}
