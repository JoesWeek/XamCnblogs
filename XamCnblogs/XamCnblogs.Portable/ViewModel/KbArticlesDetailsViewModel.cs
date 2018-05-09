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
using Microsoft.AppCenter.Crashes;

namespace XamCnblogs.Portable.ViewModel
{
    public class KbArticlesDetailsViewModel : ViewModelBase
    {
        private KbArticles articles;

        KbArticlesDetailsModel articlesDetailsModel;
        public KbArticlesDetailsModel KbArticlesDetails
        {
            get { return articlesDetailsModel; }
            set { SetProperty(ref articlesDetailsModel, value); }
        }
        public KbArticlesDetailsViewModel(KbArticles articles)
        {
            this.articles = articles;
            Title = "知识库";
            KbArticlesDetails = new KbArticlesDetailsModel()
            {
                DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐",
                ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读"
            };
        }
        bool hasError;
        public bool HasError
        {
            get { return hasError; }
            set { SetProperty(ref hasError, value); }
        }

        public async Task<KbArticles> RefreshKbArticlesAsync()
        {
            try
            {
                IsBusy = true;
                HasError = false;
                var result = await StoreManager.KbArticlesDetailsService.GetKbArticlesAsync(articles.Id);
                if (result.Success)
                {
                    articles.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                    KbArticlesDetails.DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐";
                    KbArticlesDetails.ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读";

                    HasError = false;
                }
                else
                {
                    Crashes.TrackError(new Exception() { Source = result.Message });
                    HasError = true;
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

        public class KbArticlesDetailsModel : BaseViewModel
        {
            string diggDisplay;
            public string DiggDisplay
            {
                get { return diggDisplay; }
                set { SetProperty(ref diggDisplay, value); }
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
