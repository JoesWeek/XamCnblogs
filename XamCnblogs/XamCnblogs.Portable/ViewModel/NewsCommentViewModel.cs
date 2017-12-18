using XamCnblogs.Portable.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class NewsCommentViewModel : ViewModelBase
    {
        Action<string> cloceAction;
        int id;
        public NewsCommentViewModel( int id, Action<string> cloceAction)
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
                     var result = await StoreManager.NewsCommentService.PostCommentAsync(id, content.ToString());
                     if (result.Success)
                     {
                         cloceAction.Invoke(content.ToString());
                         Toast.SendToast("评论成功");
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
