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
    public partial class QuestionsAnswersPopupPage : PopupPage
    {
        QuestionsAnswersViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsAnswersViewModel);
        QuestionsAnswersViewModel vm;
        Action<QuestionsAnswers> result;
        int questionId;
        public QuestionsAnswersPopupPage(int questionId,Action<QuestionsAnswers> result)
        {
            this.questionId = questionId;
            this.result = result;
            InitializeComponent();
            BindingContext = new QuestionsAnswersViewModel(questionId, new Action<string>(OnClose));
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
                QuestionsAnswers cmment = new QuestionsAnswers();
                cmment.AnswerUserInfo = new QuestionUserInfo()
                {
                    UserID = UserSettings.Current.SpaceUserId,
                    IconName = UserSettings.Current.Avatar,
                    UCUserID = UserSettings.Current.UserId,
                    UserName = UserSettings.Current.DisplayName,
                    QScore = UserSettings.Current.Score
                };
                cmment.Answer = result;
                cmment.DateAdded = DateTime.Now;
                cmment.Qid = questionId;
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