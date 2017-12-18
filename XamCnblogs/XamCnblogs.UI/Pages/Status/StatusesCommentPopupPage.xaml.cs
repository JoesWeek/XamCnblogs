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
        StatusesCommentViewModel ViewModel => vm ?? (vm = BindingContext as StatusesCommentViewModel);
        StatusesCommentViewModel vm;
        Action<StatusesComments> result;
        int id;
        public StatusesCommentPopupPage(int id, Action<StatusesComments> result)
        {
            this.id = id;
            this.result = result;
            InitializeComponent();
            BindingContext = new StatusesCommentViewModel(id, new Action<string>(OnClose));
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
                StatusesComments cmment = new StatusesComments();
                cmment.UserDisplayName = UserSettings.Current.DisplayName;
                cmment.UserIconUrl = UserSettings.Current.Avatar;
                cmment.Content = result;
                cmment.DateAdded = DateTime.Now;
                cmment.StatusId = id;
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