
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace XamCnblogs.Portable.Services
{
    public class QuestionsService : IQuestionsService
    {
        public QuestionsService()
        {
        }
        public async Task<ResponseMessage> GetQuestionsAsync(int position, int pageIndex = 1, int pageSize = 20)
        {
            string url = "";
            switch (position)
            {
                case 0:
                    url = string.Format(Apis.Questions, pageIndex, pageSize);
                    break;
                case 1:
                    url = string.Format(Apis.QuestionsType, "highscore", pageIndex, pageSize);
                    break;
                case 2:
                    url = string.Format(Apis.QuestionsType, "noanswer", pageIndex, pageSize);
                    break;
                case 3:
                    url = string.Format(Apis.QuestionsType, "solved", pageIndex, pageSize);
                    break;
                case 4:
                    url = string.Format(Apis.QuestionsType, "myquestion", pageIndex, pageSize);
                    break;
            }
            return position == 4 ? await UserHttpClient.Current.GetAsyn(url) : await TokenHttpClient.Current.GetAsyn(url);
        }
        public async Task<ResponseMessage> EditQuestionsAsync(Questions questions)
        {
            var url = "";
            var parameters = new Dictionary<string, string>();
            parameters.Add("Title", questions.Title);
            parameters.Add("Content", questions.Content);
            parameters.Add("Tags", questions.TagsDisplay);
            parameters.Add("Flags", "1");
            parameters.Add("UserID", questions.QuestionUserInfo.UserID.ToString());

            if (questions.Qid > 0)
            {
                url = string.Format(Apis.QuestionEdit, questions.Qid);
                return await UserHttpClient.Current.PatchAsync(url, new FormUrlEncodedContent(parameters));
            }
            else
            {
                url = string.Format(Apis.QuestionADD);
                return await UserHttpClient.Current.PostAsync(url, new FormUrlEncodedContent(parameters));
            }
        }
    }
}
