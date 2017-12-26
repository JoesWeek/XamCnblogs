
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
    public class ArticlesDetailsService : IArticlesDetailsService
    {
        public ArticlesDetailsService()
        {
        }
        public async Task<ResponseMessage> GetArticlesAsync(int id)
        {
            var url = string.Format(Apis.ArticleBody, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> GetCommentAsync(string blogApp, int id, int pageIndex, int pageSize = 20)
        {
            var url = string.Format(Apis.ArticleComment, blogApp, id, pageIndex, pageSize);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> PostCommentAsync(string blogApp, int id, string content)
        {
            var url = string.Format(Apis.ArticleCommentAdd, blogApp, id);

            var parameters = new Dictionary<string, string>();
            parameters.Add("body", content);

            return await UserHttpClient.Current.PostAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
    }
}
