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
    public class NewsDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<NewsComments> NewsComments { get; } = new ObservableRangeCollection<NewsComments>();
        private News news;
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;

        NewsDetailsModel newsDetailsModel;
        public NewsDetailsModel NewsDetails
        {
            get { return newsDetailsModel; }
            set { SetProperty(ref newsDetailsModel, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }

        public NewsDetailsViewModel(News news)
        {
            this.news = news;
            Title = news.Title;
            NewsDetails = new NewsDetailsModel()
            {
                HasContent = false,
                DiggDisplay = news.DiggCount > 0 ? news.DiggCount.ToString() : "推荐",
                CommentDisplay = news.CommentCount > 0 ? news.CommentCount.ToString() : "评论",
                ViewDisplay = news.ViewCount > 0 ? news.ViewCount.ToString() : "阅读",
                DateDisplay = "发布于 " + news.DateDisplay
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
                    var result = await StoreManager.NewsDetailsService.GetNewsAsync(news.Id);
                    if (result.Success)
                    {
                        news.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                        NewsDetails.Title = news.Title;
                        NewsDetails.Content = news.BodyDisplay;
                        NewsDetails.DiggDisplay = news.DiggCount > 0 ? news.DiggCount.ToString() : "推荐";
                        NewsDetails.CommentDisplay = news.CommentCount > 0 ? news.CommentCount.ToString() : "评论";
                        NewsDetails.ViewDisplay = news.ViewCount > 0 ? news.ViewCount.ToString() : "阅读";
                        NewsDetails.DateDisplay = "发布于 " + news.DateDisplay;
                        NewsDetails.HasError = false;
                        NewsDetails.HasContent = true;

                        await ExecuteCommentCommandAsync();
                    }
                    else
                    {
                        Log.SaveLog("NewsDetailsViewModel.GetNewsAsync" , new Exception() { Source = result.Message });
                        NewsDetails.HasError = true;
                        NewsDetails.HasContent = false;
                        LoadStatus = LoadMoreStatus.StausDefault;
                        CanLoadMore = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.SaveLog("NewsDetailsViewModel.RefreshCommand" , ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }));
        ICommand loadMoreCommand;
        public ICommand LoadMoreCommand =>
            loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
            {
                try
                {
                    if (!NewsDetails.HasError)
                    {
                        LoadStatus = LoadMoreStatus.StausLoading;

                        await ExecuteCommentCommandAsync();
                    }
                }
                catch (Exception)
                {
                    LoadStatus = LoadMoreStatus.StausError;
                }
            }));
        async Task ExecuteCommentCommandAsync()
        {
            var result = await StoreManager.NewsDetailsService.GetCommentAsync(news.Id, pageIndex, pageSize);
            if (result.Success)
            {
                var news = JsonConvert.DeserializeObject<List<NewsComments>>(result.Message.ToString());
                if (news.Count > 0)
                {
                    if (pageIndex == 1 && NewsComments.Count > 0)
                        NewsComments.Clear();
                    NewsComments.AddRange(news);
                    pageIndex++;
                    if (NewsComments.Count >= pageSize)
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
                    CanLoadMore = false;
                    LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
                }
            }
            else
            {
                Log.SaveLog("NewsDetailsViewModel.GetCommentAsync", new Exception() { Source = result.Message });
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }
        public async Task<bool> ExecuteCommentEditCommandAsync(int id, string content, bool hasEdit = false)
        {
            var result = await StoreManager.NewsDetailsService.PostCommentAsync(id, content.ToString(), hasEdit);
            if (result.Success)
            {
                Toast.SendToast(hasEdit ? "修改评论成功" : "评论成功");
            }
            else
            {
                Log.SaveLog("NewsDetailsViewModel.PostCommentAsync", new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public void AddComment(NewsComments comment)
        {
            var book = NewsComments.Where(b => b.CommentID == comment.CommentID).FirstOrDefault();
            if (book == null)
            {
                NewsComments.Add(comment);
                NewsDetails.CommentDisplay = (news.CommentCount + 1).ToString();
            }
            else
            {
                var index = NewsComments.IndexOf(book);
                NewsComments[index] = comment;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }
        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<NewsComments>(async (comment) =>
            {
                var index = NewsComments.IndexOf(comment);
                if (!NewsComments[index].IsDelete)
                {
                    NewsComments[index].IsDelete = true;
                    var result = await StoreManager.NewsDetailsService.DeleteCommentAsync(comment.CommentID);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = NewsComments.IndexOf(comment);
                        NewsComments.RemoveAt(index);
                        if (NewsComments.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                        NewsDetails.CommentDisplay = (news.CommentCount - 1).ToString();
                    }
                    else
                    {
                        Log.SaveLog("NewsDetailsViewModel.DeleteCommentAsync",  new Exception() { Source = result.Message });
                        index = NewsComments.IndexOf(comment);
                        NewsComments[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
        public class NewsDetailsModel : BaseViewModel
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
            string content;
            public string Content
            {
                get { return content; }
                set { SetProperty(ref content, value); }
            }
            string dateDisplay;
            public string DateDisplay
            {
                get { return dateDisplay; }
                set { SetProperty(ref dateDisplay, value); }
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
