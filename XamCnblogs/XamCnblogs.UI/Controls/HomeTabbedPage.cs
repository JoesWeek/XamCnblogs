using Xamarin.Forms;
using XamCnblogs.UI.Pages.Account;
using XamCnblogs.UI.Pages.Article;
using XamCnblogs.UI.Pages.New;
using XamCnblogs.UI.Pages.Question;
using XamCnblogs.UI.Pages.Status;

namespace XamCnblogs.UI.Controls {
    public class HomeTabbedPage : TabbedPage {
        bool hasInitialization;
        public HomeTabbedPage() {

        }
        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                this.Children.Add(new ArticlesTopTabbedPage());
                this.Children.Add(new NewsTopTabbedPage());
                this.Children.Add(new StatusesTopTabbedPage());
                this.Children.Add(new QuestionsTopTabbedPage());
                this.Children.Add(new AccountPage());

                hasInitialization = true;
            }
        }
    }
}
