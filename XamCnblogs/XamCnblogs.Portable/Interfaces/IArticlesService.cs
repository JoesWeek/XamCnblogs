using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IArticlesService
    {
        Task<ResponseMessage> GetArticlesAsync(int position, int pageIndex = 1);
    }
}
