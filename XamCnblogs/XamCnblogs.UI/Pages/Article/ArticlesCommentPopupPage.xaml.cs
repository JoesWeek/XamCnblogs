using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Article
{
    public partial class ArticlesCommentPopupPage : PopupPage
    {
        ArticlesCommentViewModel ViewModel => vm ?? (vm = BindingContext as ArticlesCommentViewModel);
        ArticlesCommentViewModel vm;
        Action<ArticlesComments> result;
        int id;
        string blogApp;
        public ArticlesCommentPopupPage(string blogApp, int id, Action<ArticlesComments> result)
        {
            this.blogApp = blogApp;
            this.id = id;
            this.result = result;
            InitializeComponent();
            BindingContext = new ArticlesCommentViewModel(blogApp, id, new Action<string>(OnClose));
            this.Comment.Focus();
        }
        private void OnClose(object sender, EventArgs e)
        {
            OnClose(null);
        }
        private void OnClose(string result)
        {
            if (result != null)
            {
                ArticlesComments cmment = new ArticlesComments();
                cmment.Author = UserSettings.Current.DisplayName;
                cmment.AuthorUrl = UserSettings.Current.Avatar;
                cmment.FaceUrl = UserSettings.Current.Face;
                cmment.Body = result;
                cmment.DateAdded = DateTime.Now;
                cmment.Floor = 0;
                cmment.Id = 0;
                this.result.Invoke(cmment);
            }
            PopupNavigation.PopAsync();
        }
        void OnSendComment(object sender, EventArgs args)
        {
            var toast = DependencyService.Get<IToast>();
            var comment = this.Comment.Text;
            if (comment == null)
            {
                toast.SendToast("说点什么吧.");
            }
            else if (comment.Length < 5)
            {
                toast.SendToast("多说一点吧.");
            }
            else
            {
                ViewModel.CommentCommand.Execute(comment);
            }
        }
    }
}