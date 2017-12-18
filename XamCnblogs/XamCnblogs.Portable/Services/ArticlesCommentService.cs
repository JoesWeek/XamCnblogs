
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;


namespace XamCnblogs.Portable.Services
{
    public class ArticlesCommentService : IArticlesCommentService
    {
        int pageSize = 10;
        public ArticlesCommentService()
        {
        }
        public async Task<ResponseMessage> GetCommentAsync(string blogApp, int id, int pageIndex)
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
