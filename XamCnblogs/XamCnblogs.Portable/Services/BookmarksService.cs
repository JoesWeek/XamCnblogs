
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class BookmarksService : IBookmarksService
    {
        public BookmarksService()
        {
        }
        public async Task<ResponseMessage> GetBookmarksAsync(int pageIndex = 1, int pageSize = 20)
        {
            var url = string.Format(Apis.Bookmarks,  pageIndex, pageSize);
            return await UserHttpClient.Current.GetAsyn(url);
        }
    }
}
