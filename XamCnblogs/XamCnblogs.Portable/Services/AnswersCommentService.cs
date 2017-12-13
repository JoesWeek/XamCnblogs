
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class AnswersCommentService : IAnswersCommentService
    {
        public AnswersCommentService()
        {
        }
        public async Task<ResponseMessage> GetCommentAsync(int id)
        {
            var url = string.Format(Apis.QuestionsAnswerComments,  id);
            return await TokenHttpClient.Current.GetAsyn(url);
        }
    }
}
