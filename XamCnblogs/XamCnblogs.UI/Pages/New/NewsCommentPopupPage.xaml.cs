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

namespace XamCnblogs.UI.Pages.New
{
	public partial class NewsCommentPopupPage : PopupPage
    {
        NewsCommentViewModel ViewModel => vm ?? (vm = BindingContext as NewsCommentViewModel);
        NewsCommentViewModel vm;
        Action<NewsComments> result;
        int id;
        public NewsCommentPopupPage(int id, Action<NewsComments> result)
        {
            this.id = id;
            this.result = result;
            InitializeComponent();
            BindingContext = new NewsCommentViewModel(id, new Action<string>(OnClose));
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
                NewsComments cmment = new NewsComments();
                cmment.UserName = UserSettings.Current.DisplayName;
                cmment.FaceUrl = UserSettings.Current.Avatar;
                cmment.CommentContent = result;
                cmment.DateAdded = DateTime.Now;
                cmment.Floor = 0;
                cmment.CommentID = 0;
                cmment.AgreeCount = 0;
                cmment.AntiCount = 0;
                cmment.ContentID = 0;
                cmment.UserGuid = UserSettings.Current.UserId;
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