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
                var search = SearchsListView.SelectedItem as Search;
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
                var articlesDetails = new NewsDetailsPage(news);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.SearchsListView.SelectedItem = null;
            };
        }        
    }
}