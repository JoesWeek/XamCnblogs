using FormsToolkit;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Account;
using XamCnblogs.UI.Pages.New;

namespace XamCnblogs.UI.Pages.Article
{
    public partial class ArticlesDetailsPage : ContentPage
    {
        ArticlesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as ArticlesDetailsViewModel);
        ArticlesDetailsViewModel vm;
        Articles articles;
        public ArticlesDetailsPage(Articles articles)
        {
            this.articles = articles;
            InitializeComponent();
            BindingContext = new ArticlesDetailsViewModel(articles);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares(articles.Url, articles.Title);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";

            var v = ArticlesDetailsView.HeaderTemplate.Values;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdatePage();
        }
        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.Now > (ViewModel?.NextRefreshTime ?? DateTime.Now));

            if (forceRefresh)
            {
                //刷新
                ViewModel.RefreshCommand.Execute(null);
            }
            else
            {
                //加载本地数据
                if (ViewModel.ArticlesComments.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
        void OnScrollComment(object sender, EventArgs args)
        {
            if (ViewModel.ArticlesComments.Count > 0)
                ArticlesDetailsView.ScrollTo(ViewModel.ArticlesComments.First(), ScrollToPosition.Start, false);
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new ArticlesCommentPopupPage(articles, new Action<ArticlesComments>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        private void OnResult(ArticlesComments result)
        {
            if (result != null)
            {
                ViewModel.AddComment(result);
                ArticlesDetailsView.ScrollTo(ViewModel.ArticlesComments.Last(), ScrollToPosition.Start, false);
            }
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = articles.Title, LinkUrl = articles.Url, FromCNBlogs = true }));
            }
        }
    }
}