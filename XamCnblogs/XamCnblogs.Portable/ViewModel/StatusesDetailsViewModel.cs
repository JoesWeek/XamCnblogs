using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class StatusesDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<StatusesComments> Comments { get; } = new ObservableRangeCollection<StatusesComments>();
        Statuses statuses;

        public Statuses Statuses
        {
            get { return statuses; }
            set { SetProperty(ref statuses, value); }
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
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    var result = await StoreManager.StatusesCommentsService.GetStatusesCommentsAsync(statuses.Id);
                    if (result.Success)
                    {
                        var comments = JsonConvert.DeserializeObject<List<StatusesComments>>(result.Message.ToString());
                        if (comments.Count > 0)
                        {
                            if (Comments.Count > 0)
                                Comments.Clear();
                            Comments.AddRange(comments);
                            LoadStatus = LoadMoreStatus.StausEnd;
                        }
                        else
                        {
                            LoadStatus = LoadMoreStatus.StausNodata;
                        }
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausError;
                        if (Comments.Count > 0)
                            Comments.Clear();
                    }
                    CanLoadMore = false;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    IsBusy = false;
                }
            }));
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
    }
}
