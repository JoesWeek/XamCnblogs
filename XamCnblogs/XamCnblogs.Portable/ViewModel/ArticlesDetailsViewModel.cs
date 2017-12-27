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
using Humanizer;
using System.Globalization;

namespace XamCnblogs.Portable.ViewModel
{
    public class ArticlesDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<ArticlesComments> ArticlesComments { get; } = new ObservableRangeCollection<ArticlesComments>();
        private Articles articles;
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;

        ArticlesDetailsModel articlesDetailsModel;
        public ArticlesDetailsModel ArticlesDetails
        {
            get { return articlesDetailsModel; }
            set { SetProperty(ref articlesDetailsModel, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }

        public ArticlesDetailsViewModel(Articles articles)
        {
            this.articles = articles;
            Title = articles.Title;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            ArticlesDetails = new ArticlesDetailsModel()
            {
                HasContent = false,
                DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐",
                CommentDisplay = articles.CommentCount > 0 ? articles.CommentCount.ToString() : "评论",
                ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读",
                DateDisplay = "发布于 " + articles.DateDisplay
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
                        var result = await StoreManager.ArticlesDetailsService.GetArticlesAsync(articles.Id);
                        if (result.Success)
                        {
                            articles.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                            ArticlesDetails.Title = articles.Title;
                            ArticlesDetails.Author = articles.Author;
                            ArticlesDetails.Avatar = articles.Avatar;
                            ArticlesDetails.Content = articles.BodyDisplay;
                            ArticlesDetails.DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐";
                            ArticlesDetails.CommentDisplay = articles.CommentCount > 0 ? articles.CommentCount.ToString() : "评论";
                            ArticlesDetails.ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读";
                            ArticlesDetails.DateDisplay = "发布于 " + articles.DateDisplay;
                            ArticlesDetails.HasError = false;
                            ArticlesDetails.HasContent = true;

                            await ExecuteCommentCommandAsync();
                        }
                        else
                        {
                            ArticlesDetails.HasError = true;
                            ArticlesDetails.HasContent = false;
                            LoadStatus = LoadMoreStatus.StausDefault;
                            CanLoadMore = false;
                            if (ArticlesComments.Count > 0)
                                ArticlesComments.Clear();
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
        ICommand loadMoreCommand;
        public ICommand LoadMoreCommand =>
            loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
            {
                try
                {
                    if (!ArticlesDetails.HasError)
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
            var result = await StoreManager.ArticlesDetailsService.GetCommentAsync(articles.BlogApp, articles.Id, pageIndex, pageSize);
            if (result.Success)
            {
                var comments = JsonConvert.DeserializeObject<List<ArticlesComments>>(result.Message.ToString());
                if (comments.Count > 0)
                {
                    if (pageIndex == 1 && ArticlesComments.Count > 0)
                        ArticlesComments.Clear();
                    ArticlesComments.AddRange(comments);
                    pageIndex++;
                    if (ArticlesComments.Count >= pageSize)
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
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }

        public async Task<bool> ExecuteCommentEditCommandAsync(string blogApp, int id, string content)
        {
            var result = await StoreManager.ArticlesDetailsService.PostCommentAsync(blogApp, id, content);
            if (result.Success)
            {
                Toast.SendToast("评论成功");
            }
            else
            {
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public void AddComment(ArticlesComments comment)
        {
            ArticlesComments.Add(comment);
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
            ArticlesDetails.CommentDisplay = (articles.CommentCount + 1).ToString();
        }
        public class ArticlesDetailsModel : BaseViewModel
        {
            string author;
            public string Author
            {
                get { return author; }
                set { SetProperty(ref author, value); }
            }
            string avatar;
            public string Avatar
            {
                get { return avatar; }
                set { SetProperty(ref avatar, value); }
            }
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
            private bool isDelete;
            public bool IsDelete
            {
                get { return isDelete; }
                set { SetProperty(ref isDelete, value); }
            }
        }
    }
}
