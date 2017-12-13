using Naxam.Controls.Forms;

using Xamarin.Forms;

namespace XamCnblogs.UI.Pages.Article
{
    public class ArticlesTopTabbedPage : TopTabbedPage
    {
        public ArticlesTopTabbedPage()
        {
            BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            BarIndicatorColor = (Color)Application.Current.Resources["Divider"];
            BarBackgroundColor = (Color)Application.Current.Resources["Primary"];
            
            Title = "首页";
            Icon = "menu_blog.png";
            this.Children.Add(new ArticlesPage() { Title = "博客" });
            this.Children.Add(new ArticlesPage(1) { Title = "精华" });
        }
    }
}