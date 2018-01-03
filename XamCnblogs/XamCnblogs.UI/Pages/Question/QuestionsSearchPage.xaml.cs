using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
namespace XamCnblogs.UI.Pages.Question
{
    public partial class QuestionsSearchPage : ContentPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public QuestionsSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(3);
            this.SearchsListView.ItemSelected += async delegate
            {
                var articles = SearchsListView.SelectedItem as Questions;
                if (articles == null)
                    return;

                var articlesDetails = new QuestionsDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }        
    }
}