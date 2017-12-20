using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IArticlesDetailsService
    {
        Task<ResponseMessage> GetArticlesAsync(int id);
        Task<ResponseMessage> HeadBookmarksAsync(string link);
    }
}
