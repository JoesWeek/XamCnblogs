using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class BookmarksEditPage : ContentPage
    {
        BookmarksViewModel ViewModel => vm ?? (vm = BindingContext as BookmarksViewModel);
        BookmarksViewModel vm;
        Bookmarks bookmarks;
        ActivityIndicatorPopupPage popupPage;
        Action<Bookmarks> result;
        public BookmarksEditPage(Bookmarks bookmarks) : base()
        {
            Init(bookmarks);
        }
        public BookmarksEditPage(Bookmarks bookmarks, Action<Bookmarks> result) : base()
        {
            this.result = result;
            Init(bookmarks);
        }
        void Init(Bookmarks bookmarks)
        {
            this.bookmarks = bookmarks;
            InitializeComponent();
            BindingContext = new BookmarksViewModel();
            if (bookmarks.WzLinkId > 0)
            {
                Title = "编辑收藏";
            }
            else
            {
                Title = "添加新收藏";
            }
            var cancel = new ToolbarItem
            {
                Text = "保存",
                Command = new Command(async () =>
                {
                    await ExecuteBookmarkEditAsync();
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "menu_send.png";

            this.EditorSummary.Text = bookmarks.Summary;
            this.EntryTitle.Text = bookmarks.Title;
            this.EntryLink.Text = bookmarks.LinkUrl;
            this.EntryTags.Text = bookmarks.TagsDisplay;
        }
        public async Task ExecuteBookmarkEditAsync()
        {
            var toast = DependencyService.Get<IToast>();
            var title = this.EntryTitle.Text;
            var link = this.EntryLink.Text;
            var tags = this.EntryTags.Text;
            var summary = this.EditorSummary.Text;
            if (title == null)
            {
                toast.SendToast("请输入标题");
            }
            else if (title.Length < 3)
            {
                toast.SendToast("标题最少要3个字.");
            }
            else if (link == null)
            {
                toast.SendToast("请输入网址.");
            }
            else if (link.Length < 3)
            {
                toast.SendToast("网址最少要3个字.");
            }
            else
            {
                bookmarks.Title = title;
                if (tags != null)
                    bookmarks.Tags = tags.Split(',').ToList();
                bookmarks.LinkUrl = link;
                bookmarks.Summary = summary;
                bookmarks.DateAdded = DateTime.Now;

                if (popupPage == null)
                {
                    popupPage = new ActivityIndicatorPopupPage();
                }
                await Navigation.PushPopupAsync(popupPage);
                
                if (await ViewModel.ExecuteBookmarkEditCommandAsync(bookmarks))
                {
                    await Navigation.RemovePopupPageAsync(popupPage);
                    if (result != null)
                        result.Invoke(bookmarks);
                    Navigation.RemovePage(this);
                }
                else
                {
                    await Navigation.RemovePopupPageAsync(popupPage);
                }
            }
        }
    }
}