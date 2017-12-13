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
    public class NewsDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<NewsComment> NewsComments { get; } = new ObservableRangeCollection<NewsComment>();
        private News news;
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;

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
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            this.news = news;
            Title = news.Title;
            NewsDetails = new NewsDetailsModel()
            {
                HasContent = false,
                DiggDisplay = "推荐 " + news.DiggCount,
                CommentDisplay = news.CommentCount.ToString(),
                ViewDisplay = "阅读 " + news.ViewCount,
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
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    await Task.Run(async () =>
                    {
                        var result = await StoreManager.NewsDetailsService.GetNewsAsync(news.Id);
                        if (result.Success)
                        {
                            news.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                            NewsDetails.Title = news.Title;
                            NewsDetails.Content = news.Body;
                            NewsDetails.DiggDisplay = "推荐 " + news.DiggCount;
                            NewsDetails.CommentDisplay = news.CommentCount.ToString();
                            NewsDetails.ViewDisplay = "阅读 " + news.ViewCount;
                            NewsDetails.DateDisplay = "发布于 " + news.DateDisplay;
                            NewsDetails.HasError = false;
                            NewsDetails.HasContent = true;

                            await ExecuteCommentCommandAsync();
                        }
                        else
                        {
                            NewsDetails.HasError = true;
                            NewsDetails.HasContent = false;
                            LoadStatus = LoadMoreStatus.StausDefault;
                            CanLoadMore = false;
                            if (NewsComments.Count > 0)
                                NewsComments.Clear();
                        }
                    });
                }
                catch
                {
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
            await Task.Delay(2000);
            var result = await StoreManager.NewsCommentService.GetCommentAsync(news.Id, pageIndex);
            if (result.Success)
            {
                var news = JsonConvert.DeserializeObject<List<NewsComment>>(result.Message.ToString());
                if (news.Count > 0)
                {
                    if (pageIndex == 1 && NewsComments.Count > 0)
                        NewsComments.Clear();
                    NewsComments.AddRange(news);
                    pageIndex++;
                    LoadStatus = LoadMoreStatus.StausDefault;
                    CanLoadMore = true;
                }
                else
                {
                    CanLoadMore = false;
                    LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
                }
            }
            else
            {
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
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
