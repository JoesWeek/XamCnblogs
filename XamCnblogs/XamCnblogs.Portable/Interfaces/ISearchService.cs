using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Interfaces
{
    public interface ISearchService
    {
        Task<ResponseMessage> GetSearchAsync(int position, string keyWords, int pageIndex = 1, int pageSize = 20);
    }
}
