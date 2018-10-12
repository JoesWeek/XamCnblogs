using Naxam.Controls.Forms;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.UI.Pages.New {
    public class NewsTopTabbedPage : TopTabbedPage {
        bool hasInitialization;
        public NewsTopTabbedPage() {
            Title = "新闻";
            Icon = "menu_news.png";

            BarTextColor = (Color)Application.Current.Resources["TitleText"];
            BarIndicatorColor = (Color)Application.Current.Resources["TitleText"];
            BarBackgroundColor = (Color)Application.Current.Resources["NavigationText"];

            if (Device.iOS == Device.RuntimePlatform) {
                var cancel = new ToolbarItem {
                    Text = "搜索",
                    Command = new Command(async () => {
                        await NavigationService.PushAsync(Navigation, new NewsSearchPage());
                    }),
                    Icon = "toolbar_search.png"
                };
                ToolbarItems.Add(cancel);
            }
        }
        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                this.Children.Add(new NewsPage() { Title = "最新新闻" });
                this.Children.Add(new NewsPage(1) { Title = "推荐新闻" });
                this.Children.Add(new NewsPage(2) { Title = "本周热门" });

                hasInitialization = true;
            }
        }
    }
}
