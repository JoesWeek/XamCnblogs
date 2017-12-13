
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;

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
    }
}
