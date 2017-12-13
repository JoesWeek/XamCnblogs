
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;

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
    }
}
