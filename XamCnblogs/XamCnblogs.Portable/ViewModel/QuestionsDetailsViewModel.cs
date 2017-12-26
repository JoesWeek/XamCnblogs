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
using System.Linq;

namespace XamCnblogs.Portable.ViewModel
{
    public class QuestionsDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<QuestionsAnswers> QuestionAnswers { get; } = new ObservableRangeCollection<QuestionsAnswers>();
        Questions questions;
        private int pageIndex = 1;
        private int pageSize = 20;
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
                    pageIndex = 1;
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
            var result = await StoreManager.QuestionsDetailsService.GetAnswersAsync(questions.Qid, pageIndex, pageSize);
            if (result.Success)
            {
                var answers = JsonConvert.DeserializeObject<List<QuestionsAnswers>>(result.Message.ToString());
                if (answers.Count > 0)
                {
                    if (pageIndex == 1 && QuestionAnswers.Count > 0)
                        QuestionAnswers.Clear();
                    QuestionAnswers.AddRange(answers);
                    pageIndex++;
                    if (QuestionAnswers.Count >= pageSize)
                    {
                        LoadStatus = LoadMoreStatus.StausDefault;
                        CanLoadMore = true;
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausEnd;
                        CanLoadMore = false;
                    }
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

        public async Task<bool> ExecuteCommentPostCommandAsync(int id, string content)
        {
            var result = await StoreManager.QuestionsDetailsService.PostAnswerAsync(id, content.ToString());
            if (result.Success)
            {
                Toast.SendToast("回答成功");
            }
            else
            {
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public async Task<bool> ExecuteCommentEditCommandAsync(int questionId, int answerId, int userId, string content)
        {
            var result = await StoreManager.QuestionsDetailsService.EditAnswerAsync(questionId, answerId, userId, content.ToString());
            if (result.Success)
            {
                Toast.SendToast("修改回答成功");
            }
            else
            {
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<QuestionsAnswers>(async (comment) =>
            {
                var index = QuestionAnswers.IndexOf(comment);
                if (!QuestionAnswers[index].IsDelete)
                {
                    QuestionAnswers[index].IsDelete = true;
                    var result = await StoreManager.QuestionsDetailsService.DeleteAnswerAsync(comment.Qid, comment.AnswerID);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = QuestionAnswers.IndexOf(comment);
                        QuestionAnswers.RemoveAt(index);
                        if (QuestionAnswers.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                        QuestionsDetails.CommentDisplay = (questions.AnswerCount - 1).ToString();
                    }
                    else
                    {
                        index = QuestionAnswers.IndexOf(comment);
                        QuestionAnswers[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
        public void EditComment(QuestionsAnswers answers)
        {
            var book = QuestionAnswers.Where(b => b.AnswerID == answers.AnswerID).FirstOrDefault();
            if (book == null)
            {
                QuestionAnswers.Add(answers);
                QuestionsDetails.CommentDisplay = (questions.AnswerCount + 1).ToString();
            }
            else
            {
                var index = QuestionAnswers.IndexOf(book);
                QuestionAnswers[index] = answers;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
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
