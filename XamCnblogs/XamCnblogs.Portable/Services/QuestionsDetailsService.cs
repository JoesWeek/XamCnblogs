
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
    public class QuestionsDetailsService : IQuestionsDetailsService
    {
        public QuestionsDetailsService()
        {
        }
        public async Task<ResponseMessage> GetQuestionsAsync(int id)
        {
            var url = string.Format(Apis.QuestionDetails, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> GetAnswersAsync(int id, int pageIndex, int pageSize)
        {
            var url = string.Format(Apis.QuestionsAnswers, id, pageIndex, pageSize);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> PostAnswerAsync(int id, string content)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("Answer", content);
            var url = string.Format(Apis.QuestionsAnswers, id);
            return await UserHttpClient.Current.PostAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
        public async Task<ResponseMessage> EditAnswerAsync(int questionId, int answerId, int userId, string content)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("Answer", content);
            parameters.Add("UserID", userId.ToString());
            var url = string.Format(Apis.QuestionsAnswerEdit, questionId, answerId);
            return await UserHttpClient.Current.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
        public async Task<ResponseMessage> DeleteAnswerAsync(int questionId, int answerId)
        {
            var url = string.Format(Apis.QuestionsAnswerEdit, questionId, answerId);
            return await UserHttpClient.Current.DeleteAsync(url);
        }
    }
}
