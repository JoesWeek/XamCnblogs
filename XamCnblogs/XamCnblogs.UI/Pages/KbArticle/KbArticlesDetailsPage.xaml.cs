using FormsToolkit;
using Newtonsoft.Json;
using System;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Account;

namespace XamCnblogs.UI.Pages.KbArticle
{
    public partial class KbArticlesDetailsPage : ContentPage
    {
        KbArticlesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as KbArticlesDetailsViewModel);
        KbArticlesDetailsViewModel vm;
        KbArticles kbArticles;

        public KbArticlesDetailsPage(KbArticles kbArticles)
        {
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);
            this.kbArticles = kbArticles;
            BindingContext = new KbArticlesDetailsViewModel(kbArticles);

            var bookmarks = new ToolbarItem {
                Text = "收藏",
                Command = new Command(async () => {
                    if (UserTokenSettings.Current.HasExpiresIn()) {
                        MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                    }
                    else {
                        var url = "http://kb.cnblogs.com/page/" + kbArticles.Id + "/";
                        await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = kbArticles.Title, LinkUrl = url, FromCNBlogs = true }));
                    }
                }),
                Icon = "icon_bookmarks.png"
            };
            ToolbarItems.Add(bookmarks);

            if (Device.Android == Device.RuntimePlatform) {
                var cancel = new ToolbarItem
                {
                    Text = "分享",
                    Command = new Command(() =>
                    {
                        DependencyService.Get<IShares>().Shares("http://kb.cnblogs.com/page/" + kbArticles.Id + "/", kbArticles.Title);
                    }),
                    Icon = "toolbar_share.png"
                };
                ToolbarItems.Add(cancel);
            }

            formsWebView.OnContentLoaded += delegate (object sender, EventArgs e)
            {
                RefreshKbArticles();
            };
        }
        async void RefreshKbArticles()
        {
            var question = JsonConvert.SerializeObject(await ViewModel.RefreshKbArticlesAsync());
            await formsWebView.InjectJavascriptAsync("updateModel(" + question + ");");
        }
        void OnReloadKbArticles(object sender, EventArgs args)
        {
            RefreshKbArticles();
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
        }
    }
}