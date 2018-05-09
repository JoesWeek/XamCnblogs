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
        QuestionsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsDetailsViewModel);
        QuestionsDetailsViewModel vm;
        Action<QuestionsAnswers> result;
        Questions questions;
        QuestionsAnswers questionsAnswers;
        public QuestionsAnswersPopupPage(Questions questions, Action<QuestionsAnswers> result, QuestionsAnswers questionsAnswers = null)
        {
            this.questions = questions;
            this.questionsAnswers = questionsAnswers;
            this.result = result;
            InitializeComponent();
            BindingContext = new QuestionsDetailsViewModel(questions);
            if (questionsAnswers != null)
            {
                this.Comment.Text = questionsAnswers.Answer;
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
                if (questionsAnswers == null)
                {
                    questionsAnswers = new QuestionsAnswers();
                    questionsAnswers.AnswerUserInfo = new QuestionUserInfo()
                    {
                        UserID = UserSettings.Current.SpaceUserId,
                        IconName = UserSettings.Current.Avatar,
                        UCUserID = UserSettings.Current.UserId,
                        UserName = UserSettings.Current.DisplayName,
                        QScore = UserSettings.Current.Score
                    };
                    questionsAnswers.Qid = questions.Qid;
                }
                questionsAnswers.Answer = result;
                questionsAnswers.DateAdded = DateTime.Now;
                this.result.Invoke(questionsAnswers);
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
                if (questionsAnswers == null)
                {
                    if (AboutSettings.Current.WeibaToggled)
                        comment += "<br/>" + AboutSettings.Current.WeibaContent;

                    if (await ViewModel.ExecuteCommentPostCommandAsync(questions.Qid, comment))
                    {
                        SendButton.IsRunning = false;
                        ClosePopupPage(comment);
                    }
                    else
                    {
                        SendButton.IsRunning = false;
                    }
                }
                else
                {
                    if (await ViewModel.ExecuteCommentEditCommandAsync(questions.Qid, questionsAnswers.AnswerID, questionsAnswers.UserID, comment))
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
}