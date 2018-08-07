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

        public KbArticlesDetailsViewModel(KbArticles articles)
        {
            this.articles = articles;
            Title = "知识库";
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
    }
}
