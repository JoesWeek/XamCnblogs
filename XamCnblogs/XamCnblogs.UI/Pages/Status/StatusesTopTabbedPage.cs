using Naxam.Controls.Forms;
using Xamarin.Forms;

namespace XamCnblogs.UI.Pages.Status {
    public class StatusesTopTabbedPage : TopTabbedPage {
        bool hasInitialization;
        public StatusesTopTabbedPage() {
            BarTextColor = (Color)Application.Current.Resources["TitleText"];
            BarIndicatorColor = (Color)Application.Current.Resources["TitleText"];
            BarBackgroundColor = (Color)Application.Current.Resources["NavigationText"];

            Title = "闪存";
            Icon = "menu_statuses.png";
        }
        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                this.Children.Add(new StatusesPage() { Title = "全站" });
                this.Children.Add(new StatusesPage(1) { Title = "关注" });
                this.Children.Add(new StatusesPage(2) { Title = "我的" });
                this.Children.Add(new StatusesPage(3) { Title = "我回应" });
                this.Children.Add(new StatusesPage(6) { Title = "回复我" });

                hasInitialization = true;
            }
        }
    }
}
