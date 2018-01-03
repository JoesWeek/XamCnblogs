using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
namespace XamCnblogs.UI.Pages.KbArticle
{
    public partial class KbArticlesSearchPage : ContentPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public KbArticlesSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(2);
            this.SearchsListView.ItemSelected += async delegate
            {
                var articles = SearchsListView.SelectedItem as KbArticles;
                if (articles == null)
                    return;

                var articlesDetails = new KbArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }        
    }
}