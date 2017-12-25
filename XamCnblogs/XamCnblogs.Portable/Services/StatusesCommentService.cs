
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
    public class StatusesCommentService : IStatusesCommentService
    {
        public StatusesCommentService()
        {
        }
        public async Task<ResponseMessage> GetCommentsAsync(int id)
        {
            var url = string.Format(Apis.StatusComments, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> PostCommentAsync(int id, string content)
        {
            var url = string.Format(Apis.StatusCommentAdd, id);

            var parameters = new Dictionary<string, string>();
            parameters.Add("ReplyTo", "0");
            parameters.Add("ParentCommentId", "0");
            parameters.Add("Content", content);

            return await UserHttpClient.Current.PostAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
        public async Task<ResponseMessage> DeleteCommentAsync(int statusId, int id)
        {
            var url = string.Format(Apis.StatusCommentDelete, statusId, id);

            return await UserHttpClient.Current.DeleteAsync(url);
        }
    }
}
