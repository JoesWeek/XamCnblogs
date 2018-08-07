using Naxam.Controls.Forms;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.UI.Pages.New
{
    public class NewsTopTabbedPage : TopTabbedPage
    {
        public NewsTopTabbedPage()
        {
            Title = "新闻";
            Icon = "menu_news.png";

            this.Children.Add(new NewsPage() { Title = "最新新闻" });
            this.Children.Add(new NewsPage(1) { Title = "推荐新闻" });
            this.Children.Add(new NewsPage(2) { Title = "本周热门" });

            if (Device.iOS == Device.RuntimePlatform)
            {
                BarTextColor = (Color)Application.Current.Resources["PrimaryText"];
                BarIndicatorColor = (Color)Application.Current.Resources["SecondaryText"];
                BarBackgroundColor = (Color)Application.Current.Resources["NavigationText"];

                var cancel = new ToolbarItem
                {
                    Text = "搜索",
                    Command = new Command(async () =>
                    {
                        await NavigationService.PushAsync(Navigation, new NewsSearchPage());
                    }),
                    Icon = "toolbar_search.png"
                };
                ToolbarItems.Add(cancel);
            }
            else
            {
                BarTextColor = (Color)Application.Current.Resources["NavigationText"];
                BarIndicatorColor = (Color)Application.Current.Resources["Divider"];
                BarBackgroundColor = (Color)Application.Current.Resources["Primary"];
            }
        }
    }
}
