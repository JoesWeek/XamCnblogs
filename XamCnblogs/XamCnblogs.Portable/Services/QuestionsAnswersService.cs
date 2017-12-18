
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
    public class QuestionsAnswersService : IQuestionsAnswersService
    {
        public QuestionsAnswersService()
        {
        }
        public async Task<ResponseMessage> GetAnswersAsync(int id)
        {
            var url = string.Format(Apis.QuestionsAnswers, id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
        
        public async Task<ResponseMessage> PostAnswersAsync(int id, string content)
        {
            var url = string.Format(Apis.QuestionsAnswers, id);

            var parameters = new Dictionary<string, string>();
            parameters.Add("Answer", content);

            return await UserHttpClient.Current.PostAsync(url, new StringContent(JsonConvert.SerializeObject(parameters), Encoding.UTF8, "application/json"));
        }
    }
}
