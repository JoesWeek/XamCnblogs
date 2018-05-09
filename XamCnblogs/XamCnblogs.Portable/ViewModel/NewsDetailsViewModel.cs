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
using Microsoft.AppCenter.Crashes;

namespace XamCnblogs.Portable.ViewModel
{
    public class NewsDetailsViewModel : ViewModelBase
    {
        public List<NewsComments> NewsComments { get; } = new List<NewsComments>();
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
        bool hasError;
        public bool HasError
        {
            get { return hasError; }
            set { SetProperty(ref hasError, value); }
        }


        public NewsDetailsViewModel(News news)
        {
            this.news = news;
            Title = "新闻";
            NewsDetails = new NewsDetailsModel()
            {
                DiggDisplay = news.DiggCount > 0 ? news.DiggCount.ToString() : "推荐",
                CommentDisplay = news.CommentCount > 0 ? news.CommentCount.ToString() : "评论",
                ViewDisplay = news.ViewCount > 0 ? news.ViewCount.ToString() : "阅读"
            };
            IsBusy = true;
        }
        public async Task<News> RefreshNewsAsync()
        {
            try
            {
                IsBusy = true;
                pageIndex = 1;
                HasError = false;
                NextRefreshTime = DateTime.Now.AddMinutes(15);
                var result = await StoreManager.NewsDetailsService.GetNewsAsync(news.Id);
                if (result.Success)
                {
                    news.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                    NewsDetails.DiggDisplay = news.DiggCount > 0 ? news.DiggCount.ToString() : "推荐";
                    NewsDetails.CommentDisplay = news.CommentCount > 0 ? news.CommentCount.ToString() : "评论";
                    NewsDetails.ViewDisplay = news.ViewCount > 0 ? news.ViewCount.ToString() : "阅读";
                    NewsDetails.DateDisplay = "发布于 " + news.DateDisplay;
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
            return news;
        }

        public async Task<List<NewsComments>> ReloadCommentsAsync()
        {
            try
            {
                LoadStatus = LoadMoreStatus.StausLoading;

                var result = await StoreManager.NewsDetailsService.GetCommentAsync(news.Id, pageIndex, pageSize);
                if (result.Success)
                {
                    var newsComments = JsonConvert.DeserializeObject<List<NewsComments>>(result.Message.ToString());
                    if (newsComments.Count > 0)
                    {
                        if (pageIndex == 1)
                            NewsComments.Clear();
                        NewsComments.AddRange(newsComments);
                        pageIndex++;
                        if (newsComments.Count < pageSize)
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
            return NewsComments;
        }

        public async Task<bool> EditCommentAsync(int id, string content, bool hasEdit = false)
        {
            var result = await StoreManager.NewsDetailsService.PostCommentAsync(id, content.ToString(), hasEdit);
            if (result.Success)
            {
                Toast.SendToast(hasEdit ? "修改评论成功" : "评论成功");
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public async Task<bool> DeleteCommentAsync(int id)
        {
            var newsComments = NewsComments.Where(n => n.CommentID == id).FirstOrDefault();
            var result = await StoreManager.NewsDetailsService.DeleteCommentAsync(newsComments.CommentID);
            if (result.Success)
            {
                var index = NewsComments.IndexOf(newsComments);
                NewsComments.RemoveAt(index);
                if (NewsComments.Count == 0)
                    LoadStatus = LoadMoreStatus.StausNodata;
                NewsDetails.CommentDisplay = (news.CommentCount = news.CommentCount - 1).ToString();
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast("删除失败");
            }
            return result.Success;
        }

        public void EditComment(NewsComments comment)
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
        }
    }
}
