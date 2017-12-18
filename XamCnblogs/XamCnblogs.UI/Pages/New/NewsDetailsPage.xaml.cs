using FormsToolkit;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.New
{
    public partial class NewsDetailsPage : ContentPage
    {
        NewsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as NewsDetailsViewModel);
        NewsDetailsViewModel vm;
        News news;
        public NewsDetailsPage(News news)
        {
            this.news = news;
            InitializeComponent();
            BindingContext = new NewsDetailsViewModel(news);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.RefreshCommand.Execute(null);
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
        void OnScrollComment(object sender, EventArgs args)
        {
            if (ViewModel.NewsComments.Count > 0)
                NewsDetailsView.ScrollTo(ViewModel.NewsComments.First(), ScrollToPosition.Start, false);
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new NewsCommentPopupPage(news.Id, new Action<NewsComments>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        private void OnResult(NewsComments result)
        {
            if (result != null)
            {
                ViewModel.AddComment(result);
                NewsDetailsView.ScrollTo(ViewModel.NewsComments.Last(), ScrollToPosition.Start, false);
            }
        }
    }
}