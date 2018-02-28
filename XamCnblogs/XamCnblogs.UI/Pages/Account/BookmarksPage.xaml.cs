using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Article;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class BookmarksPage : ContentPage
    {
        BookmarksViewModel ViewModel => vm ?? (vm = BindingContext as BookmarksViewModel);
        BookmarksViewModel vm;
        public BookmarksPage() : base()
        {
            InitializeComponent();
            BindingContext = new BookmarksViewModel();

            var cancel = new ToolbarItem
            {
                Text = "添加",
                Command = new Command(async () =>
                {
                    await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks(), new Action<Bookmarks>(OnResult)));
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_add.png";

            this.BookmarksListView.ItemSelected += async delegate
            {
                var bookmarks = BookmarksListView.SelectedItem as Bookmarks;
                if (bookmarks == null)
                    return;

                var bookmarksDetailsPage = new BookmarksDetailsPage(bookmarks);

                await NavigationService.PushAsync(Navigation, bookmarksDetailsPage);
                this.BookmarksListView.SelectedItem = null;
            };
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
                if (ViewModel.Bookmarks.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
        private void OnResult(Bookmarks result)
        {
            if (result != null)
            {
                ViewModel.EditBookmark(result);
                BookmarksListView.ScrollTo(ViewModel.Bookmarks.FirstOrDefault(), ScrollToPosition.Start, false);
            }
        }
        public ICommand EditCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    await NavigationService.PushAsync(Navigation, new BookmarksEditPage(e as Bookmarks, new Action<Bookmarks>(OnResult)));
                });
            }
        }
    }
}