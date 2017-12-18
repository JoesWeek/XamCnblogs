using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IAnswersCommentService
    {
        Task<ResponseMessage> GetCommentAsync(int id);
        Task<ResponseMessage> PostCommentAsync(int questionId, int answerId, string content);
    }
}
