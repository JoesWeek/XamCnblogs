using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Microsoft.AppCenter.Crashes;

namespace XamCnblogs.Portable.ViewModel
{
    public class StatusesDetailsViewModel : ViewModelBase
    {
        public List<StatusesComments> StatusesComments { get; } = new List<StatusesComments>();
        Statuses statuses;

        public Statuses Statuses
        {
            get { return statuses; }
            set { SetProperty(ref statuses, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
        private string comment;
        public string CommentDisplay
        {
            get { return comment; }
            set { SetProperty(ref comment, value); }
        }
        public StatusesDetailsViewModel(Statuses statuses)
        {
            this.statuses = statuses;
            CanLoadMore = false;
            CommentDisplay = statuses.CommentCount > 0 ? statuses.CommentCount.ToString() : "评论";
        }
        public async Task<List<StatusesComments>> ReloadCommentsAsync()
        {
            try
            {
                var result = await StoreManager.StatusesCommentsService.GetCommentsAsync(statuses.Id);
                if (result.Success)
                {
                    var comments = JsonConvert.DeserializeObject<List<StatusesComments>>(result.Message.ToString());
                    if (comments.Count > 0)
                    {
                        if (StatusesComments.Count > 0)
                            StatusesComments.Clear();
                        StatusesComments.AddRange(comments);
                        LoadStatus = LoadMoreStatus.StausEnd;
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausNodata;
                    }
                }
                else
                {
                    Crashes.TrackError(new Exception() { Source = result.Message });
                    LoadStatus = LoadMoreStatus.StausError;
                }
                CanLoadMore = false;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return StatusesComments;
        }

        public async Task<bool> ExecuteCommentEditCommandAsync(int id, string content)
        {
            var result = await StoreManager.StatusesCommentsService.PostCommentAsync(id, content.ToString());
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
        public async Task<bool> DeleteStatusesCommentsAsync(int id)
        {
            var statusesComments = StatusesComments.Where(n => n.Id == id).FirstOrDefault();
            var result = await StoreManager.StatusesCommentsService.DeleteCommentAsync(statusesComments.StatusId, statusesComments.Id);
            if (result.Success)
            {
                var index = StatusesComments.IndexOf(statusesComments);
                StatusesComments.RemoveAt(index);
                if (StatusesComments.Count == 0)
                    LoadStatus = LoadMoreStatus.StausNodata;
                CommentDisplay = (Statuses.CommentCount = Statuses.CommentCount - 1).ToString();
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast("删除失败");
            }
            return result.Success;
        }
        public void AddComment(StatusesComments comment)
        {
            StatusesComments.Add(comment);
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
            CommentDisplay = (Statuses.CommentCount = Statuses.CommentCount + 1).ToString();
        }
    }
}
