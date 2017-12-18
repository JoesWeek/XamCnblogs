using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IStatusesCommentService
    {
        Task<ResponseMessage> GetStatusesCommentsAsync(int id);
        Task<ResponseMessage> PostCommentAsync(int id, string content);
    }
}
