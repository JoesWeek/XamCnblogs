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
    public class AnswersDetailsViewModel : ViewModelBase
    {
        public List<AnswersComments> AnswersComments { get; } = new List<AnswersComments>();
        QuestionsAnswers answers;

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
            AnswersDetails = new AnswersDetailsModel()
            {
                DiggDisplay = answers.DiggCount > 0 ? answers.DiggCount.ToString() : "推荐",
                CommentDisplay = answers.CommentCounts > 0 ? answers.CommentCounts.ToString() : "评论"
            };
        }
        public async Task<List<AnswersComments>> ReloadCommentsAsync()
        {
            LoadStatus = LoadMoreStatus.StausLoading;

            var result = await StoreManager.AnswersDetailsService.GetCommentAsync(answers.AnswerID);
            if (result.Success)
            {
                var comments = JsonConvert.DeserializeObject<List<AnswersComments>>(result.Message.ToString());
                if (comments.Count > 0)
                {
                    if (AnswersComments.Count > 0)
                        AnswersComments.Clear();
                    AnswersComments.AddRange(comments);
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
                Crashes.TrackError(new Exception() { Source = result.Message });
                LoadStatus = LoadMoreStatus.StausError;
            }
            return AnswersComments;
        }

        public async Task<bool> ExecuteCommentPostCommandAsync(int questionId, int answerId, string content)
        {
            var result = await StoreManager.AnswersDetailsService.PostCommentAsync(questionId, answerId, content);
            if (result.Success)
            {
                Toast.SendToast("评论成功");
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
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
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }

        public async Task<bool> DeleteAnswersCommentsAsync(int id)
        {
            var answersComments = AnswersComments.Where(n => n.CommentID == id).FirstOrDefault();
            var result = await StoreManager.AnswersDetailsService.DeleteCommentAsync(answers.Qid, answers.AnswerID, answersComments.CommentID);
            if (result.Success)
            {
                var index = AnswersComments.IndexOf(answersComments);
                AnswersComments.RemoveAt(index);
                if (AnswersComments.Count == 0)
                    LoadStatus = LoadMoreStatus.StausNodata;
                AnswersDetails.CommentDisplay = (answers.CommentCounts = answers.CommentCounts - 1).ToString();
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast("删除失败");
            }
            return result.Success;
        }
        public void EditComment(AnswersComments comment)
        {
            var book = AnswersComments.Where(b => b.CommentID == comment.CommentID).FirstOrDefault();
            if (book == null)
            {
                AnswersComments.Add(comment);
                AnswersDetails.CommentDisplay = (answers.CommentCounts = answers.CommentCounts + 1).ToString();
            }
            else
            {
                var index = AnswersComments.IndexOf(book);
                AnswersComments[index] = comment;
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
