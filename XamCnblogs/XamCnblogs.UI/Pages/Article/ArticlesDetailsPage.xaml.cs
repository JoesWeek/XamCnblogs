
using FormsToolkit;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Account;

namespace XamCnblogs.UI.Pages.Article {
    public partial class ArticlesDetailsPage : ContentPage {
        ArticlesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as ArticlesDetailsViewModel);
        ArticlesDetailsViewModel vm;
        Articles articles;
        ArticlesCommentPopupPage popupPage;
        public ArticlesDetailsPage(Articles articles) {
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);

            this.articles = articles;

            BindingContext = new ArticlesDetailsViewModel(articles);

            if (Device.Android == Device.RuntimePlatform) {
                var cancel = new ToolbarItem {
                    Text = "分享",
                    Command = new Command(() => {
                        DependencyService.Get<IShares>().Shares(articles.Url, articles.Title);
                    }),
                    Icon = "toolbar_share.png"
                };
                ToolbarItems.Add(cancel);
            }
            formsWebView.OnContentLoaded += delegate (object sender, EventArgs e) {
                RefreshArticles();
            };
            formsWebView.AddLocalCallback("reload", async delegate (string obj) {
                if (formsWebView.LoadStatus == LoadMoreStatus.StausDefault || formsWebView.LoadStatus == LoadMoreStatus.StausError || formsWebView.LoadStatus == LoadMoreStatus.StausFail) {
                    var articlesComments = JsonConvert.SerializeObject(await ViewModel.ReloadCommentsAsync());
                    await formsWebView.InjectJavascriptAsync("updateComments(" + articlesComments + ");");
                }
            });
        }

        async void RefreshArticles() {
            var model = JsonConvert.SerializeObject(await ViewModel.RefreshArticlesAsync());
            await formsWebView.InjectJavascriptAsync("updateModel(" + model + ");");
        }

        void OnReloadArticles(object sender, EventArgs args) {
            RefreshArticles();
        }

        async void OnScrollComment(object sender, EventArgs args) {
            await formsWebView.InjectJavascriptAsync("scrollToComments();");
        }

        async void OnShowComment(object sender, EventArgs args) {
            if (UserTokenSettings.Current.HasExpiresIn()) {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else {
                popupPage = new ArticlesCommentPopupPage(articles, new Action<ArticlesComments>(OnResult));
                if (popupPage != null && Navigation != null)
                    await Navigation.PushPopupAsync(popupPage);
            }
        }

        private async void OnResult(ArticlesComments result) {
            if (result != null) {
                await formsWebView.InjectJavascriptAsync("updateComment(" + JsonConvert.SerializeObject(result) + ");");
            }
        }

        async void OnBookmarks(object sender, EventArgs args) {
            if (UserTokenSettings.Current.HasExpiresIn()) {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else {
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = articles.Title, LinkUrl = articles.Url, FromCNBlogs = true }));
            }
        }

        protected override bool OnBackButtonPressed() {
            if (popupPage != null) {
                Navigation.RemovePopupPageAsync(popupPage);
                popupPage = null;
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}