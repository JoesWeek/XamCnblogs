using Naxam.Controls.Forms;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.UI.Pages.KbArticle;

namespace XamCnblogs.UI.Pages.Article
{
    public class ArticlesTopTabbedPage : TopTabbedPage
    {
        public ArticlesTopTabbedPage()
        {
            Title = "首页";
            Icon = "menu_home.png";
            this.Children.Add(new ArticlesPage() { Title = "博客" });
            this.Children.Add(new ArticlesPage(1) { Title = "精华" });
            this.Children.Add(new KbArticlesPage() { Title = "知识库" });

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
                        await NavigationService.PushAsync(Navigation, new ArticlesSearchPage());
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