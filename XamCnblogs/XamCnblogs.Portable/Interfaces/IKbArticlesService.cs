using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IKbArticlesService
    {
        Task<ResponseMessage> GetKbArticlesAsync(int pageIndex = 1,int pageSize = 20);
    }
}
