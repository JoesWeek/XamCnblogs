
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class StatusesService : IStatusesService
    {
        private int pageSize = 10;
        public StatusesService()
        {
        }
        public async Task<ResponseMessage> GetStatusesAsync(int position, int pageIndex = 1)
        {
            string statusType = "all";
            switch (position)
            {
                case 0:
                    statusType = "all";
                    break;
                case 1:
                    statusType = "following";
                    break;
                case 2:
                    statusType = "my";
                    break;
                case 3:
                    statusType = "mycomment";
                    break;
                case 4:
                    statusType = "recentcomment";
                    break;
                case 5:
                    statusType = "mention";
                    break;
                case 6:
                    statusType = "comment";
                    break;
                default:
                    statusType = "all";
                    break;
            }
            var url = string.Format(Apis.Status, statusType, pageIndex, pageSize);
            if (position > 0)
            {
                return await UserHttpClient.Current.GetAsyn(url);
            }
            else
            {
                return await TokenHttpClient.Current.GetAsyn(url);
            }
        }
    }
}
