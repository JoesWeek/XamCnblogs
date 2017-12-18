using Naxam.Controls.Forms;
using Xamarin.Forms;

namespace XamCnblogs.UI.Pages.New
{
    public class NewsTopTabbedPage : TopTabbedPage
    {
        public NewsTopTabbedPage()
        {
            BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            BarIndicatorColor = (Color)Application.Current.Resources["Divider"];
            BarBackgroundColor = (Color)Application.Current.Resources["Primary"];

            Title = "新闻";
            Icon = "menu_news.png";

            this.Children.Add(new NewsPage() { Title = "最新新闻" });
            this.Children.Add(new NewsPage(1) { Title = "推荐新闻" });
            this.Children.Add(new NewsPage(2) { Title = "本周热门" });
        }
    }
}
