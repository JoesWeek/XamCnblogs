using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface ICommentService
    {
        Task<ResponseMessage> PostComment(int id, string content);
    }
}
