
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using System.Threading.Tasks;


namespace XamCnblogs.Portable.Services
{
    public class QuestionsService : IQuestionsService
    {
        private int pageSize = 10;
        public QuestionsService()
        {
        }
        public async Task<ResponseMessage> GetQuestionsAsync(int position, int pageIndex = 1)
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
    }
}
