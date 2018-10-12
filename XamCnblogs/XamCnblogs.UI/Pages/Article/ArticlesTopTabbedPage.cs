using Naxam.Controls.Forms;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.UI.Pages.KbArticle;

namespace XamCnblogs.UI.Pages.Article
{
    public class ArticlesTopTabbedPage : TopTabbedPage {
        bool hasInitialization;
        public ArticlesTopTabbedPage()
        {
            Title = "首页";
            Icon = "menu_home.png";

            BarTextColor = (Color)Application.Current.Resources["TitleText"];
            BarIndicatorColor = (Color)Application.Current.Resources["TitleText"];
            BarBackgroundColor = (Color)Application.Current.Resources["NavigationText"];

            if (Device.iOS == Device.RuntimePlatform)
            {
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
        }
        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                this.Children.Add(new ArticlesPage() { Title = "博客" });
                this.Children.Add(new ArticlesPage(1) { Title = "精华" });
                this.Children.Add(new KbArticlesPage() { Title = "知识库" });

                hasInitialization = true;
            }
        }
    }
}