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
        ArticlesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as ArticlesDetailsViewModel);
        ArticlesDetailsViewModel vm;
        Action<ArticlesComments> result;
        Articles articles;
        public ArticlesCommentPopupPage(Articles articles, Action<ArticlesComments> result)
        {
            this.articles = articles;
            this.result = result;
            InitializeComponent();
            BindingContext = new ArticlesDetailsViewModel(articles);
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

                if (AboutSettings.Current.WeibaToggled)
                    comment += "<br/>" + AboutSettings.Current.WeibaContent;

                if (await ViewModel.ExecuteCommentEditCommandAsync(articles.BlogApp, articles.Id, comment))
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