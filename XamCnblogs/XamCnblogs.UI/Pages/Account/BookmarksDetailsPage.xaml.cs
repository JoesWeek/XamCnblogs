using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class BookmarksDetailsPage : ContentPage
    {
        public BookmarksDetailsPage(Bookmarks bookmarks) : base()
        {
            InitializeComponent();
            Title = bookmarks.Title;

            if (Device.Android == Device.RuntimePlatform)
            {
                var cancel = new ToolbarItem
                {
                    Text = "分享",
                    Command = new Command(() =>
                    {
                        DependencyService.Get<IShares>().Shares(bookmarks.LinkUrl, bookmarks.Title);
                    }),
                    Icon = "toolbar_share.png"
                };
                ToolbarItems.Add(cancel);
            }

            authorizeView.Source = bookmarks.LinkUrl;
        }
    }
}