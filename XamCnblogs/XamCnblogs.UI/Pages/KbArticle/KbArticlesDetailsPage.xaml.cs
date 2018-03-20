using FormsToolkit;
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
            this.kbArticles = kbArticles;
            InitializeComponent();
            BindingContext = new KbArticlesDetailsViewModel(kbArticles);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares("http://kb.cnblogs.com/page/" + kbArticles.Id + "/", kbArticles.Title);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";
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
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var url = "http://kb.cnblogs.com/page/" + kbArticles.Id + "/";
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = kbArticles.Title, LinkUrl = url, FromCNBlogs = true }));
            }
        }
    }
}