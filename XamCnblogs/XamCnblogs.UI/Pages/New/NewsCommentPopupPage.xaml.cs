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
        NewsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as NewsDetailsViewModel);
        NewsDetailsViewModel vm;
        Action<NewsComments> result;
        News news;
        NewsComments comments;
        public NewsCommentPopupPage(News news, Action<NewsComments> result, NewsComments comments = null)
        {
            this.news = news;
            this.comments = comments;
            this.result = result;
            InitializeComponent();
            BindingContext = new NewsDetailsViewModel(news);
            if (comments != null)
            {
                this.Comment.Text = comments.CommentContent;
            }
            this.Comment.Focus();
            ViewModel.IsBusy = false;
        }
        private void OnClose(object sender, EventArgs e)
        {
            ClosePopupPage(null);
        }
        private void ClosePopupPage(string result)
        {
            if (result != null)
            {
                if (comments == null)
                {
                    comments = new NewsComments();
                    comments.UserName = UserSettings.Current.DisplayName;
                    comments.FaceUrl = UserSettings.Current.Avatar;
                    comments.Floor = 0;
                    comments.CommentID = 0;
                    comments.AgreeCount = 0;
                    comments.AntiCount = 0;
                    comments.ContentID = 0;
                    comments.UserGuid = UserSettings.Current.UserId;
                }
                comments.CommentContent = result;
                comments.DateAdded = DateTime.Now;
                this.result.Invoke(comments);
            }
            PopupNavigation.PopAsync();
        }
        async void OnSendComment(object sender, EventArgs args)
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
                SendButton.IsRunning = true;

                if (AboutSettings.Current.WeibaToggled && comments == null)
                    comment += "<br/>" + AboutSettings.Current.WeibaContent;

                if (await ViewModel.EditCommentAsync(news.Id, comment, comments != null))
                {
                    SendButton.IsRunning = false;
                    ClosePopupPage(comment);
                }
                else
                {
                    SendButton.IsRunning = false;
                }
            }
        }
    }
}