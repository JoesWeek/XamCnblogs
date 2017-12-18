using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.Services;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class QuestionsDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<QuestionsAnswers> QuestionAnswers { get; } = new ObservableRangeCollection<QuestionsAnswers>();
        Questions questions;
        public DateTime NextRefreshTime { get; set; }

        QuestionsDetailsModel questionsDetails;
        public QuestionsDetailsModel QuestionsDetails
        {
            get { return questionsDetails; }
            set { SetProperty(ref questionsDetails, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }

        public QuestionsDetailsViewModel(Questions questions)
        {
            this.questions = questions;
            Title = questions.Title;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            QuestionsDetails = new QuestionsDetailsModel()
            {
                HasContent = false,
                DiggDisplay = questions.DiggCount > 0 ? questions.DiggCount.ToString() : "推荐",
                CommentDisplay = questions.AnswerCount > 0 ? questions.AnswerCount.ToString() : "回答",
                ViewDisplay = questions.ViewCount > 0 ? questions.ViewCount.ToString() : "阅读"
            };
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    await Task.Run(async () =>
                    {
                        var result = await StoreManager.QuestionsDetailsService.GetQuestionsAsync(questions.Qid);
                        if (result.Success)
                        {
                            questions = JsonConvert.DeserializeObject<Questions>(result.Message.ToString());

                            QuestionsDetails.Title = questions.Title;
                            QuestionsDetails.UserName = questions.QuestionUserInfo.UserName;
                            QuestionsDetails.UserDisplay = HtmlTemplate.GetScoreName(questions.QuestionUserInfo.QScore) + " · " + questions.QuestionUserInfo.QScore + "园豆" + " · 提问于 " + questions.DateDisplay;
                            QuestionsDetails.Content = questions.ContentDisplay;
                            QuestionsDetails.IconDisplay = questions.QuestionUserInfo.IconDisplay;
                            QuestionsDetails.Award = questions.Award;
                            QuestionsDetails.TagsDisplay = questions.TagsDisplay;
                            QuestionsDetails.DealFlag = questions.DealFlag;
                            QuestionsDetails.DiggDisplay = questions.DiggCount > 0 ? questions.DiggCount.ToString() : "推荐";
                            QuestionsDetails.CommentDisplay = questions.AnswerCount > 0 ? questions.AnswerCount.ToString() : "回答";
                            QuestionsDetails.ViewDisplay = questions.ViewCount > 0 ? questions.ViewCount.ToString() : "阅读";
                            switch (questions.DealFlag)
                            {
                                case 1:
                                    QuestionsDetails.DealFlagDisplay = "已解决";
                                    break;
                                case -1:
                                    QuestionsDetails.DealFlagDisplay = "已关闭";
                                    break;
                                default:
                                    QuestionsDetails.DealFlagDisplay = "待解决";
                                    break;
                            }
                            QuestionsDetails.HasError = false;
                            QuestionsDetails.HasContent = true;

                            await ExecuteCommentCommandAsync();
                        }
                        else
                        {
                            QuestionsDetails.HasError = true;
                            QuestionsDetails.HasContent = false;
                            LoadStatus = LoadMoreStatus.StausDefault;
                            CanLoadMore = false;
                            if (QuestionAnswers.Count > 0)
                                QuestionAnswers.Clear();
                        }
                    });
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    IsBusy = false;
                }
            }));

        async Task ExecuteCommentCommandAsync()
        {
            var result = await StoreManager.QuestionsAnswersService.GetAnswersAsync(questions.Qid);
            if (result.Success)
            {
                var comments = JsonConvert.DeserializeObject<List<QuestionsAnswers>>(result.Message.ToString());
                if (comments.Count > 0)
                {
                    if (QuestionAnswers.Count > 0)
                        QuestionAnswers.Clear();
                    QuestionAnswers.AddRange(comments);
                    LoadStatus = LoadMoreStatus.StausEnd;
                }
                else
                {
                    LoadStatus = LoadMoreStatus.StausNodata;
                }
                CanLoadMore = false;
            }
            else
            {
                LoadStatus = LoadMoreStatus.StausError;
            }
        }

        public void AddComment(QuestionsAnswers comment)
        {
            QuestionAnswers.Add(comment);
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
            QuestionsDetails.CommentDisplay = (questions.AnswerCount + 1).ToString();
        }
        public class QuestionsDetailsModel : BaseViewModel
        {
            string userName;
            public string UserName
            {
                get { return userName; }
                set { SetProperty(ref userName, value); }
            }
            string userDisplay;
            public string UserDisplay
            {
                get { return userDisplay; }
                set { SetProperty(ref userDisplay, value); }
            }
            string iconDisplay;
            public string IconDisplay
            {
                get { return iconDisplay; }
                set { SetProperty(ref iconDisplay, value); }
            }
            string diggDisplay;
            public string DiggDisplay
            {
                get { return diggDisplay; }
                set { SetProperty(ref diggDisplay, value); }
            }
            string content;
            public string Content
            {
                get { return content; }
                set { SetProperty(ref content, value); }
            }
            int award;
            public int Award
            {
                get { return award; }
                set { SetProperty(ref award, value); }
            }
            string tagsDisplay;
            public string TagsDisplay
            {
                get { return tagsDisplay; }
                set { SetProperty(ref tagsDisplay, value); }
            }
            int dealFlag;
            public int DealFlag
            {
                get { return dealFlag; }
                set { SetProperty(ref dealFlag, value); }
            }
            string dealFlagDisplay;
            public string DealFlagDisplay
            {
                get { return dealFlagDisplay; }
                set { SetProperty(ref dealFlagDisplay, value); }
            }
            string commentDisplay;
            public string CommentDisplay
            {
                get { return commentDisplay; }
                set { SetProperty(ref commentDisplay, value); }
            }
            string viewDisplay;
            public string ViewDisplay
            {
                get { return viewDisplay; }
                set { SetProperty(ref viewDisplay, value); }
            }
            bool hasError;
            public bool HasError
            {
                get { return hasError; }
                set { SetProperty(ref hasError, value); }
            }
            bool hasContent;
            public bool HasContent
            {
                get { return hasContent; }
                set { SetProperty(ref hasContent, value); }
            }
        }
    }
}
