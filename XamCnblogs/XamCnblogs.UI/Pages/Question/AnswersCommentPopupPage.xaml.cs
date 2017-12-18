using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.New
{
    public partial class AnswersCommentPopupPage : PopupPage
    {
        AnswersCommentViewModel ViewModel => vm ?? (vm = BindingContext as AnswersCommentViewModel);
        AnswersCommentViewModel vm;
        Action<AnswersComment> result;
        int questionId;
        int answerId;
        public AnswersCommentPopupPage(int questionId, int answerId, Action<AnswersComment> result)
        {
            this.questionId = questionId;
            this.answerId = answerId;
            this.result = result;
            InitializeComponent();
            BindingContext = new AnswersCommentViewModel(questionId, answerId, new Action<string>(OnClose));
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
                AnswersComment cmment = new AnswersComment();
                cmment.PostUserInfo = new QuestionUserInfo()
                {
                    UserID = UserSettings.Current.SpaceUserId,
                    IconName = UserSettings.Current.Avatar,
                    UCUserID = UserSettings.Current.UserId,
                    UserName = UserSettings.Current.DisplayName,
                    QScore = UserSettings.Current.Score
                };
                cmment.Content = result;
                cmment.DateAdded = DateTime.Now;
                cmment.CommentID = answerId;
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