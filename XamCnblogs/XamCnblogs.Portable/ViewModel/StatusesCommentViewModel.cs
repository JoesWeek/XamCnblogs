using XamCnblogs.Portable.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class StatusesCommentViewModel : ViewModelBase
    {
        Action<string> cloceAction;
        int id;
        public StatusesCommentViewModel( int id, Action<string> cloceAction)
        {
            this.id = id;
            this.cloceAction = cloceAction;
        }
        ICommand cmmentCommand;
        public ICommand CommentCommand => cmmentCommand ?? (cmmentCommand = new Command(async (content) =>
             {
                 try
                 {
                     IsBusy = true;
                     var result = await StoreManager.StatusesCommentsService.PostCommentAsync(id, content.ToString());
                     if (result.Success)
                     {
                         cloceAction.Invoke(content.ToString());
                         Toast.SendToast("回复成功");
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
