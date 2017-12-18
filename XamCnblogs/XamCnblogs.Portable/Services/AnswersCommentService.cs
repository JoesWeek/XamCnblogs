
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Services
{
    public class AnswersCommentService : IAnswersCommentService
    {
        public AnswersCommentService()
        {
        }
        public async Task<ResponseMessage> GetCommentAsync(int id)
        {
            var url = string.Format(Apis.QuestionsAnswerComments, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> PostCommentAsync(int questionId, int answerId, string content)
        {
            var url = string.Format(Apis.QuestionsAnswerCommentsAdd, questionId, answerId);

            var parameters = new Dictionary<string, string>();
            parameters.Add("Content", content);
            parameters.Add("ParentCommentId", "0");

            return await UserHttpClient.Current.PostAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
    }
}
