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
        AnswersDetailsViewModel ViewModel => vm ?? (vm = BindingContext as AnswersDetailsViewModel);
        AnswersDetailsViewModel vm;
        Action<AnswersComments> result;
        QuestionsAnswers answers;
        AnswersComments answersComment;
        public AnswersCommentPopupPage(QuestionsAnswers answers, Action<AnswersComments> result, AnswersComments answersComment = null)
        {
            this.answers = answers;
            this.result = result;
            this.answersComment = answersComment;
            InitializeComponent();
            BindingContext = new AnswersDetailsViewModel(answers);
            if (answersComment != null)
            {
                this.Comment.Text = answersComment.Content;
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
                if (answersComment == null)
                {
                    answersComment = new AnswersComments();
                    answersComment.PostUserInfo = new QuestionUserInfo()
                    {
                        UserID = UserSettings.Current.SpaceUserId,
                        IconName = UserSettings.Current.Avatar,
                        UCUserID = UserSettings.Current.UserId,
                        UserName = UserSettings.Current.DisplayName,
                        QScore = UserSettings.Current.Score
                    };
                }
                answersComment.Content = result;
                answersComment.DateAdded = DateTime.Now;
                this.result.Invoke(answersComment);
            }
            PopupNavigation.PopAsync();
        }
        async void OnSendComment(object sender, EventArgs args)
        {
            var toast = DependencyService.Get<IToast>();
            var content = this.Comment.Text;
            if (content == null)
            {
                toast.SendToast("说点什么吧.");
            }
            else if (content.Length < 5)
            {
                toast.SendToast("多说一点吧.");
            }
            else
            {
                SendButton.IsRunning = true;
                if (answersComment == null)
                {
                    if (AboutSettings.Current.WeibaToggled)
                        content += "<br/>" + AboutSettings.Current.WeibaContent;

                    if (await ViewModel.ExecuteCommentPostCommandAsync(answers.Qid, answers.AnswerID, content))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(content);
                    }
                    else
                    {
                        SendButton.IsRunning = false;
                    }
                }
                else
                {
                    if (await ViewModel.ExecuteCommentEditCommentAsync(answers.Qid, answers.AnswerID, answersComment.CommentID, answersComment.PostUserID, content))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(content);
                    }
                    else
                    {
                        SendButton.IsRunning = false;
                    }
                }
            }
        }
    }
}