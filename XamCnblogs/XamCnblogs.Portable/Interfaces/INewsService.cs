using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface INewsService
    {
        Task<ResponseMessage> GetNewsAsync(int position, int pageIndex = 1, int pageSize = 20);
    }
}
