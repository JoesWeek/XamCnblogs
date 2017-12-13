
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

            //content = @"{  'ReplyTo':0,  'ParentCommentId':0,  'Content': " + content + "}";


            var client = new HttpClient
            {
                BaseAddress = new Uri(Apis.Host)
            };
            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(UserTokenSettings.Current.UserTokenType, UserTokenSettings.Current.UserToken);
            var response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            var mess = response.Content.ReadAsStringAsync().Result;

            return await UserHttpClient.Current.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded"));

            var parameters = new Dictionary<string, string>();
            parameters.Add("ReplyTo", "0");
            parameters.Add("ParentCommentId", "0");
            parameters.Add("Content", content);

            return await UserHttpClient.Current.PostAsync(url, new FormUrlEncodedContent(parameters));
        }
    }
}
