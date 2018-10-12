using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Account {
    public partial class BookmarksPage : ContentPage {
        BookmarksViewModel ViewModel => vm ?? (vm = BindingContext as BookmarksViewModel);
        BookmarksViewModel vm;
        bool hasInitialization;
        public BookmarksPage() : base() {
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);

            var cancel = new ToolbarItem {
                Text = "添加",
                Command = new Command(async () => {
                    await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks(), new Action<Bookmarks>(OnResult)));
                }),
                Icon = "toolbar_add.png"
            };
            ToolbarItems.Add(cancel);
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                BindingContext = new BookmarksViewModel();
                this.BookmarksListView.ItemSelected += async delegate {
                    var bookmarks = BookmarksListView.SelectedItem as Bookmarks;
                    this.BookmarksListView.SelectedItem = null;
                    if (bookmarks == null)
                        return;

                    var bookmarksDetailsPage = new BookmarksDetailsPage(bookmarks);

                    await NavigationService.PushAsync(Navigation, bookmarksDetailsPage);
                };
                hasInitialization = true;
            }
            UpdatePage();
        }
        private void UpdatePage() {
            bool forceRefresh = (DateTime.Now > (ViewModel?.NextRefreshTime ?? DateTime.Now));

            if (forceRefresh) {
                //刷新
                ViewModel.RefreshCommand.Execute(null);
            }
        }
        private void OnResult(Bookmarks result) {
            if (result != null) {
                ViewModel.EditBookmark(result);
                BookmarksListView.ScrollTo(ViewModel.Bookmarks.FirstOrDefault(), ScrollToPosition.Start, false);
            }
        }
        public ICommand EditCommand {
            get {
                return new Command(async (e) => {
                    await NavigationService.PushAsync(Navigation, new BookmarksEditPage(e as Bookmarks, new Action<Bookmarks>(OnResult)));
                });
            }
        }
    }
}