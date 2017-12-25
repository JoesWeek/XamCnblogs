using XamCnblogs.Portable.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.Portable.Interfaces
{
    public interface IBookmarksService
    {
        Task<ResponseMessage> GetBookmarksAsync(int pageIndex = 1, int pageSize = 20);
        Task<ResponseMessage> EditBookmarkAsync(Bookmarks bookmarks);
        Task<ResponseMessage> DeleteBookmarkAsync(int id);
    }
}
