using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
namespace XamCnblogs.UI.Pages.New
{
    public partial class NewsSearchPage : ContentPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public NewsSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(1);
            this.SearchsListView.ItemSelected += async delegate
            {
                var articles = SearchsListView.SelectedItem as News;
                if (articles == null)
                    return;

                var articlesDetails = new NewsDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }        
    }
}