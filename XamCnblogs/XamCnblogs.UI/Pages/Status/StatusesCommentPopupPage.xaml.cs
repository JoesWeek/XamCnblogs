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
    public partial class StatusesCommentPopupPage : PopupPage
    {
        StatusesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as StatusesDetailsViewModel);
        StatusesDetailsViewModel vm;
        Statuses statuses;
        Action<StatusesComments> result;
        StatusesComments comments;
        int id;
        public StatusesCommentPopupPage(Statuses statuses, Action<StatusesComments> result)
        {
            this.statuses = statuses;
            this.result = result;
            InitializeComponent();
            BindingContext = new StatusesDetailsViewModel(statuses);
            this.Comment.Focus();
        }
        private void OnClose(object sender, EventArgs e)
        {
            ClosePopupPage(null);
        }
        private void ClosePopupPage(string result)
        {
            if (result != null)
            {
                comments = new StatusesComments();
                comments.UserDisplayName = UserSettings.Current.DisplayName;
                comments.UserIconUrl = UserSettings.Current.Avatar;
                comments.Content = result;
                comments.DateAdded = DateTime.Now;
                comments.StatusId = id;
                comments.UserGuid = UserSettings.Current.UserId;
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

                if (AboutSettings.Current.WeibaToggled)
                    comment += "<br/>" + AboutSettings.Current.WeibaContent;

                if (await ViewModel.ExecuteCommentEditCommandAsync(statuses.Id, comment))
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