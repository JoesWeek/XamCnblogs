using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.ViewModel
{
    public class ArticlesDetailsViewModel : ViewModelBase
    {
        private Articles articles;
        private int pageIndex = 1;
        private int pageSize = 20;
        public DateTime NextRefreshTime { get; set; }

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
        bool hasError;
        public bool HasError
        {
            get { return hasError; }
            set { SetProperty(ref hasError, value); }
        }

        public ArticlesDetailsViewModel(Articles articles)
        {
            Title = "博文";
            this.articles = articles;
            ArticlesDetails = new ArticlesDetailsModel()
            {
                DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐",
                CommentDisplay = articles.CommentCount > 0 ? articles.CommentCount.ToString() : "评论",
                ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读"
            };
            IsBusy = true;
        }
        public async Task<Articles> RefreshArticlesAsync()
        {
            try
            {
                IsBusy = true;
                HasError = false;
                pageIndex = 1;
                NextRefreshTime = DateTime.Now.AddMinutes(15);
                var result = await StoreManager.ArticlesDetailsService.GetArticlesAsync(articles.Id);
                if (result.Success)
                {
                    articles.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                    ArticlesDetails.DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐";
                    ArticlesDetails.CommentDisplay = articles.CommentCount > 0 ? articles.CommentCount.ToString() : "评论";
                    ArticlesDetails.ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读";
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
            return articles;
        }

        public async Task<List<ArticlesComments>> ReloadCommentsAsync()
        {
            List<ArticlesComments> articlesComments = new List<ArticlesComments>();
            try
            {
                LoadStatus = LoadMoreStatus.StausLoading;

                var result = await StoreManager.ArticlesDetailsService.GetCommentAsync(articles.BlogApp, articles.Id, pageIndex, pageSize);
                if (result.Success)
                {
                    articlesComments = JsonConvert.DeserializeObject<List<ArticlesComments>>(result.Message.ToString());
                    if (articlesComments.Count > 0)
                    {
                        pageIndex++;
                        if (articlesComments.Count < pageSize)
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
            return articlesComments;
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
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        
        public class ArticlesDetailsModel : BaseViewModel
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
