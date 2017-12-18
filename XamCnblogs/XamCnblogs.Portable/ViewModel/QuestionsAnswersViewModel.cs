using XamCnblogs.Portable.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class QuestionsAnswersViewModel : ViewModelBase
    {
        Action<string> cloceAction;
        int questionId;
        public QuestionsAnswersViewModel(int questionId, Action<string> cloceAction)
        {
            this.questionId = questionId;
            this.cloceAction = cloceAction;
        }
        ICommand cmmentCommand;
        public ICommand CommentCommand => cmmentCommand ?? (cmmentCommand = new Command(async (content) =>
             {
                 try
                 {
                     IsBusy = true;
                     var result = await StoreManager.QuestionsAnswersService.PostAnswersAsync(questionId, content.ToString());
                     if (result.Success)
                     {
                         cloceAction.Invoke(content.ToString());
                         Toast.SendToast("回答成功");
                     }
                     else
                     {
                         Toast.SendToast(result.Message.ToString());
                     }
                 }
                 catch (Exception e)
                 {
                     Toast.SendToast(e.Message);
                 }
                 finally
                 {
                     IsBusy = false;
                 }
             }
         ));

    }
}
