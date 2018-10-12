using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
namespace XamCnblogs.UI.Pages.Search
{
    public partial class KbArticlesSearchPage : BaseSearchPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public KbArticlesSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(2);
            this.SearchsListView.ItemSelected += async delegate
            {
                var search = SearchsListView.SelectedItem as Portable.Model.Search;
                this.SearchsListView.SelectedItem = null;
                if (search == null)
                    return;
                var articles = new KbArticles()
                {
                    Author = search.UserName,
                    Body = "",
                    Summary = search.Content,
                    DiggCount = search.VoteTimes,
                    Id = int.Parse(search.Id),
                    DateAdded = search.PublishTime,
                    Title = search.Title.Replace("<strong>", "").Replace("</strong>", ""),
                    ViewCount = search.ViewTimes
                };

                var articlesDetails = new KbArticle.KbArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
            };
        }
        public override async void Search(string value)
        {
            await ViewModel.ExecuteSearchCommandAsync(value);
        }
    }
}