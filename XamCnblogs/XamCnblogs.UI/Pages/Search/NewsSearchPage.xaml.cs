using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Search
{
    public partial class NewsSearchPage : BaseSearchPage
    {
        SearchViewModel ViewModel => vm ?? (vm = BindingContext as SearchViewModel);
        SearchViewModel vm;
        public NewsSearchPage() : base()
        {
            InitializeComponent();
            BindingContext = new SearchViewModel(1);
            this.SearchsListView.ItemSelected += async delegate
            {
                var search = SearchsListView.SelectedItem as Portable.Model.Search;
                this.SearchsListView.SelectedItem = null;
                if (search == null)
                    return;

                var news = new News()
                {
                    IsHot = false,
                    IsRecommend = false,
                    TopicIcon = "",
                    TopicId = 0,
                    Body = "",
                    CommentCount = search.CommentTimes,
                    Summary = search.Content,
                    DiggCount = search.VoteTimes,
                    Id = int.Parse(search.Id),
                    DateAdded =search.PublishTime,
                    Title = search.Title.Replace("<strong>", "").Replace("</strong>", ""),
                    ViewCount = search.ViewTimes
                };
                var articlesDetails = new New.NewsDetailsPage(news);

                await NavigationService.PushAsync(Navigation, articlesDetails);
            };
        }
        public override async void Search(string value)
        {
            await ViewModel.ExecuteSearchCommandAsync(value);
        }
    }
}