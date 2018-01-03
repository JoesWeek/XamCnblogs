using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
namespace XamCnblogs.UI.Pages.Article
{
    public partial class ArticlesSearchPage : ContentPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public ArticlesSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(0);
            this.SearchsListView.ItemSelected += async delegate
            {
                var articles = SearchsListView.SelectedItem as Articles;
                if (articles == null)
                    return;

                var articlesDetails = new ArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }        
    }
}