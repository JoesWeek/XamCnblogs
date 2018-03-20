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
    public class KbArticlesDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<string> KbArticlesComments { get; } = new ObservableRangeCollection<string>();
        private KbArticles articles;
        public DateTime NextRefreshTime { get; set; }

        KbArticlesDetailsModel articlesDetailsModel;
        public KbArticlesDetailsModel KbArticlesDetails
        {
            get { return articlesDetailsModel; }
            set { SetProperty(ref articlesDetailsModel, value); }
        }
        public KbArticlesDetailsViewModel(KbArticles articles)
        {
            this.articles = articles;
            Title = articles.Title;
            KbArticlesDetails = new KbArticlesDetailsModel()
            {
                DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐",
                ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读"
            };
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    KbArticlesDetails.HasError = false;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    var result = await StoreManager.KbArticlesDetailsService.GetKbArticlesAsync(articles.Id);
                    if (result.Success)
                    {
                        articles.Body = JsonConvert.DeserializeObject<string>(result.Message.ToString());

                        KbArticlesDetails.Title = articles.Title;
                        KbArticlesDetails.Content = articles.BodyDisplay;
                        KbArticlesDetails.DiggDisplay = articles.DiggCount > 0 ? articles.DiggCount.ToString() : "推荐";
                        KbArticlesDetails.ViewDisplay = articles.ViewCount > 0 ? articles.ViewCount.ToString() : "阅读";
                        KbArticlesDetails.DateDisplay = "发布与 " + articles.DateDisplay;

                        KbArticlesDetails.HasError = false;
                    }
                    else
                    {
                        Log.SaveLog("KbArticlesDetailsViewModel.GetKbArticlesAsync", new Exception() { Source = result.Message });
                        KbArticlesDetails.HasError = true;
                    }
                }
                catch (Exception ex)
                {
                    Log.SaveLog("KbArticlesDetailsViewModel.RefreshCommand", ex);
                }
                finally
                {
                    IsBusy = false;
                }
            }));
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
        }
    }
}
