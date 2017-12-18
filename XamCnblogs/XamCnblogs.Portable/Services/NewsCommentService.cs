
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace XamCnblogs.Portable.Services
{
    public class NewsCommentService : INewsCommentService
    {
        int pageSize = 10;
        public NewsCommentService()
        {
        }
        public async Task<ResponseMessage> GetCommentAsync(int id, int pageIndex)
        {
            var url = string.Format(Apis.NewsComment, id, pageIndex, pageSize);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> PostCommentAsync(int id, string content)
        {
            var url = string.Format(Apis.NewsCommentAdd, id);

            var parameters = new Dictionary<string, string>();
            parameters.Add("Content", content);

            return await UserHttpClient.Current.PostAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
    }
}
