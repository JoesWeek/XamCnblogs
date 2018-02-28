using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Article;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class BookmarksDetailsPage : ContentPage
    {
        private bool isCompleted;
        ActivityIndicatorPopupPage popupPage;

        public BookmarksDetailsPage(Bookmarks bookmarks) : base()
        {
            InitializeComponent();
            Title = bookmarks.Title;

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares(bookmarks.LinkUrl, bookmarks.Title);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";


            FormsWebView.OnNavigationCompleted += OnNavigationCompleted;
            FormsWebView.OnNavigationError += OnNavigationError;

            FormsWebView.Source = bookmarks.LinkUrl;

            popupPage = new ActivityIndicatorPopupPage();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!isCompleted)
            {
                Navigation.PushPopupAsync(popupPage);
            }
        }
        private async void OnNavigationError(object sender, int e)
        {
            await Navigation.RemovePopupPageAsync(popupPage);
        }

        private async void OnNavigationCompleted(object sender, string url)
        {
            isCompleted = true;
            await Navigation.RemovePopupPageAsync(popupPage);
        }


        protected async override void OnDisappearing()
        {
            base.OnDisappearing();

            isCompleted = true;
            await Navigation.RemovePopupPageAsync(popupPage);
        }
    }
}