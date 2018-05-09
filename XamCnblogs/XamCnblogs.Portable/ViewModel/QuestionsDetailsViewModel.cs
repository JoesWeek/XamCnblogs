using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.ViewModel
{
    public class QuestionsDetailsViewModel : ViewModelBase
    {
        public List<QuestionsAnswers> QuestionsAnswers { get; } = new List<QuestionsAnswers>();
        Questions questions;
        private int pageIndex = 1;
        private int pageSize = 20;

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
        bool hasError;
        public bool HasError
        {
            get { return hasError; }
            set { SetProperty(ref hasError, value); }
        }

        public QuestionsDetailsViewModel(Questions questions)
        {
            this.questions = questions;
            Title = "博问";
            QuestionsDetails = new QuestionsDetailsModel()
            {
                DiggDisplay = questions.DiggCount > 0 ? questions.DiggCount.ToString() : "推荐",
                CommentDisplay = questions.AnswerCount > 0 ? questions.AnswerCount.ToString() : "回答",
                ViewDisplay = questions.ViewCount > 0 ? questions.ViewCount.ToString() : "阅读"
            };
            IsBusy = true;
        }

        public async Task<Questions> RefreshQuestionsAsync()
        {
            try
            {
                IsBusy = true;
                pageIndex = 1;
                HasError = false;
                var result = await StoreManager.QuestionsDetailsService.GetQuestionsAsync(questions.Qid);
                if (result.Success)
                {
                    questions = JsonConvert.DeserializeObject<Questions>(result.Message.ToString());

                    QuestionsDetails.DiggDisplay = questions.DiggCount > 0 ? questions.DiggCount.ToString() : "推荐";
                    QuestionsDetails.CommentDisplay = questions.AnswerCount > 0 ? questions.AnswerCount.ToString() : "回答";
                    QuestionsDetails.ViewDisplay = questions.ViewCount > 0 ? questions.ViewCount.ToString() : "阅读";

                    HasError = false;
                }
                else
                {
                    HasError = true;
                    Crashes.TrackError(new Exception() { Source = result.Message });
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
            return questions;
        }

        public async Task<List<QuestionsAnswers>> ReloadAnswersAsync()
        {
            try
            {
                LoadStatus = LoadMoreStatus.StausLoading;

                var result = await StoreManager.QuestionsDetailsService.GetAnswersAsync(questions.Qid, pageIndex, pageSize);
                if (result.Success)
                {
                    var questionsAnswers = JsonConvert.DeserializeObject<List<QuestionsAnswers>>(result.Message.ToString());
                    if (questionsAnswers.Count > 0)
                    {
                        if (pageIndex == 1)
                            QuestionsAnswers.Clear();
                        QuestionsAnswers.AddRange(questionsAnswers);
                        pageIndex++;
                        if (questionsAnswers.Count < pageSize)
                        {
                            LoadStatus = LoadMoreStatus.StausEnd;
                        }
                        else
                        {
                            LoadStatus = LoadMoreStatus.StausDefault;
                        }
                    }
                    else
                    {
                        LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
                    }
                }
                else
                {
                    Crashes.TrackError(new Exception() { Source = result.Message });
                    LoadStatus = LoadMoreStatus.StausError;
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                LoadStatus = LoadMoreStatus.StausError;
            }
            return QuestionsAnswers;
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
                Crashes.TrackError(new Exception() { Source = result.Message });
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
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }

        public async Task<bool> DeleteQuestionAnswersAsync(int id)
        {
            var questionsAnswers = QuestionsAnswers.Where(n => n.AnswerID == id).FirstOrDefault();
            var result = await StoreManager.QuestionsDetailsService.DeleteAnswerAsync(questionsAnswers.Qid, questionsAnswers.AnswerID);
            if (result.Success)
            {
                var index = QuestionsAnswers.IndexOf(questionsAnswers);
                QuestionsAnswers.RemoveAt(index);
                if (QuestionsAnswers.Count == 0)
                    LoadStatus = LoadMoreStatus.StausNodata;
                QuestionsDetails.CommentDisplay = (questions.AnswerCount = questions.AnswerCount - 1).ToString();
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast("删除失败");
            }
            return result.Success;
        }

        public void EditComment(QuestionsAnswers comment)
        {
            var book = QuestionsAnswers.Where(b => b.AnswerID == comment.AnswerID).FirstOrDefault();
            if (book == null)
            {
                QuestionsAnswers.Add(comment);
                QuestionsDetails.CommentDisplay = (questions.AnswerCount + 1).ToString();
            }
            else
            {
                var index = QuestionsAnswers.IndexOf(book);
                QuestionsAnswers[index] = comment;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }
        public class QuestionsDetailsModel : BaseViewModel
        {
            string diggDisplay;
            public string DiggDisplay
            {
                get { return diggDisplay; }
                set { SetProperty(ref diggDisplay, value); }
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
        }
    }
}
