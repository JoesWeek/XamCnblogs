using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface INewsDetailsService
    {
        Task<ResponseMessage> GetNewsAsync(int id);
        Task<ResponseMessage> GetCommentAsync(int id, int pageIndex, int pageSize);
        Task<ResponseMessage> PostCommentAsync(int id, string content,bool hasEdit = false);
        Task<ResponseMessage> DeleteCommentAsync(int id);
    }
}
