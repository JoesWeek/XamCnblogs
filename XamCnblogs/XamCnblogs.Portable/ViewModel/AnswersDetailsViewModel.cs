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
    public class AnswersDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<AnswersComment> AnswersComment { get; } = new ObservableRangeCollection<AnswersComment>();
        QuestionsAnswers answers;
        public DateTime NextRefreshTime { get; set; }

        AnswersDetailsModel answersDetails;
        public AnswersDetailsModel AnswersDetails
        {
            get { return answersDetails; }
            set { SetProperty(ref answersDetails, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }

        public AnswersDetailsViewModel(QuestionsAnswers answers)
        {
            this.answers = answers;
            Title = answers.UserName + "的回答";
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            AnswersDetails = new AnswersDetailsModel();
        }
        public void ExecuteAnswersDetails()
        {
            AnswersDetails.UserName = answers.AnswerUserInfo.UserName;
            AnswersDetails.IconDisplay = answers.AnswerUserInfo.IconDisplay;
            AnswersDetails.UserDisplay = answers.UserDisplay;
            AnswersDetails.IsBest = answers.IsBest;
            AnswersDetails.Answer = answers.AnswerDisplay;
            AnswersDetails.DiggDisplay = answers.DiggCount > 0 ? answers.DiggCount.ToString() : "推荐";
            AnswersDetails.CommentDisplay = answers.CommentCounts > 0 ? answers.CommentCounts.ToString() : "评论";
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);

                    var result = await StoreManager.AnswersDetailsService.GetCommentAsync(answers.AnswerID);
                    if (result.Success)
                    {
                        var comments = JsonConvert.DeserializeObject<List<AnswersComment>>(result.Message.ToString());
                        if (comments.Count > 0)
                        {
                            if (AnswersComment.Count > 0)
                                AnswersComment.Clear();
                            AnswersComment.AddRange(comments);
                            LoadStatus = LoadMoreStatus.StausEnd;
                            CanLoadMore = false;
                        }
                        else
                        {
                            LoadStatus = LoadMoreStatus.StausNodata;
                        }
                    }
                    else
                    {
                        Log.SaveLog("AnswersDetailsViewModel.GetCommentAsync", new Exception() { Source = result.Message });
                        LoadStatus = LoadMoreStatus.StausError;
                    }
                    CanLoadMore = false;
                }
                catch (Exception ex)
                {
                    Log.SaveLog("AnswersDetailsViewModel.RefreshCommand", ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }));

        public async Task<bool> ExecuteCommentPostCommandAsync(int questionId, int answerId, string content)
        {
            var result = await StoreManager.AnswersDetailsService.PostCommentAsync(questionId, answerId, content);
            if (result.Success)
            {
                Toast.SendToast("评论成功");
            }
            else
            {
                Log.SaveLog("AnswersDetailsViewModel.PostCommentAsync" , new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public async Task<bool> ExecuteCommentEditCommentAsync(int questionId, int answerId, int commentId, int userId, string content)
        {
            var result = await StoreManager.AnswersDetailsService.EditCommentAsync(questionId, answerId, commentId, userId, content);
            if (result.Success)
            {
                Toast.SendToast("修改评论成功");
            }
            else
            {
                Log.SaveLog("AnswersDetailsViewModel.EditCommentAsync" , new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<AnswersComment>(async (comment) =>
            {
                var index = AnswersComment.IndexOf(comment);
                if (!AnswersComment[index].IsDelete)
                {
                    AnswersComment[index].IsDelete = true;
                    var result = await StoreManager.AnswersDetailsService.DeleteCommentAsync(answers.Qid, answers.AnswerID, comment.CommentID);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = AnswersComment.IndexOf(comment);
                        AnswersComment.RemoveAt(index);
                        if (AnswersComment.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                        AnswersDetails.CommentDisplay = (answers.CommentCounts - 1).ToString();
                    }
                    else
                    {
                        Log.SaveLog("AnswersDetailsViewModel.DeleteCommentAsync", new Exception() { Source = result.Message });
                        index = AnswersComment.IndexOf(comment);
                        AnswersComment[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
        public void EditComment(AnswersComment comment)
        {
            var book = AnswersComment.Where(b => b.CommentID == comment.CommentID).FirstOrDefault();
            if (book == null)
            {
                AnswersComment.Add(comment);
                AnswersDetails.CommentDisplay = (answers.CommentCounts + 1).ToString();
            }
            else
            {
                var index = AnswersComment.IndexOf(book);
                AnswersComment[index] = comment;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }
        public class AnswersDetailsModel : BaseViewModel
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
            string answer;
            public string Answer
            {
                get { return answer; }
                set { SetProperty(ref answer, value); }
            }
            bool isBest;
            public bool IsBest
            {
                get { return isBest; }
                set { SetProperty(ref isBest, value); }
            }
            string commentDisplay;
            public string CommentDisplay
            {
                get { return commentDisplay; }
                set { SetProperty(ref commentDisplay, value); }
            }
        }
    }
}
