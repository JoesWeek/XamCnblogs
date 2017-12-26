
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
    public class AnswersDetailsService : IAnswersDetailsService
    {
        public AnswersDetailsService()
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
        public async Task<ResponseMessage> EditCommentAsync(int questionId, int answerId, int commentId, int userId, string content)
        {
            var url = string.Format(Apis.QuestionsAnswerCommentsEdit, questionId, answerId, commentId);

            var parameters = new Dictionary<string, string>();
            parameters.Add("Content", content);
            parameters.Add("PostUserID", userId.ToString());

            return await UserHttpClient.Current.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
        public async Task<ResponseMessage> DeleteCommentAsync(int questionId, int answerId, int commentId)
        {
            var url = string.Format(Apis.QuestionsAnswerCommentsEdit,  questionId,  answerId,  commentId);
            return await UserHttpClient.Current.DeleteAsync(url);
        }
    }
}
