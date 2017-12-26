using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IAnswersDetailsService
    {
        Task<ResponseMessage> GetCommentAsync(int id);
        Task<ResponseMessage> PostCommentAsync(int questionId, int answerId, string content);
        Task<ResponseMessage> EditCommentAsync(int questionId, int answerId, int commentId, int userId, string content);
        Task<ResponseMessage> DeleteCommentAsync(int questionId, int answerId, int commentId); 
    }
}
