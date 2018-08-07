using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Search;

namespace XamCnblogs.UI.Pages.Search
{
    public partial class ArticlesSearchPage : BaseSearchPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public ArticlesSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(0);
            this.SearchsListView.ItemSelected += async delegate
            {
                var search = SearchsListView.SelectedItem as Portable.Model.Search;
                if (search == null)
                    return;
                var articles = new Articles()
                {
                    Author = search.UserName,
                    Avatar = search.UserName,
                    BlogApp = search.UserAlias,
                    Body = "",
                    CommentCount = search.CommentTimes,
                    Description = search.Content,
                    DiggCount = search.VoteTimes,
                    Id = int.Parse(search.Id),
                    PostDate = search.PublishTime,
                    Title = search.Title.Replace("<strong>", "").Replace("</strong>", ""),
                    Url = search.Uri,
                    ViewCount = search.ViewTimes
                };
                var articlesDetails = new Article.ArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }
        public override async void Search(string value)
        {
            await ViewModel.ExecuteSearchCommandAsync(value);
        }
    }
}