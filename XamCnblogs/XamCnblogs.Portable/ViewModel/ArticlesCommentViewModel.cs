using XamCnblogs.Portable.Model;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class ArticlesCommentViewModel : ViewModelBase
    {
        Action<string> cloceAction;
        int id;
        string blogApp;
        public ArticlesCommentViewModel(string blogApp, int id, Action<string> cloceAction)
        {
            this.id = id;
            this.blogApp = blogApp;
            this.cloceAction = cloceAction;
        }
        ICommand cmmentCommand;
        public ICommand CommentCommand => cmmentCommand ?? (cmmentCommand = new Command(async (content) =>
             {
                 try
                 {
                     IsBusy = true;
                     var result = await StoreManager.ArticlesCommentService.PostCommentAsync(blogApp, id, content.ToString());
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
