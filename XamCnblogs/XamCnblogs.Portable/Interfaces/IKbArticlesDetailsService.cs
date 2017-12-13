using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IKbArticlesDetailsService
    {
        Task<ResponseMessage> GetKbArticlesAsync(int id);
    }
}
