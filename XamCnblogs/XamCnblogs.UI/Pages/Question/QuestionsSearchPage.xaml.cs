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
                var search = SearchsListView.SelectedItem as Search;
                if (search == null)
                    return;
                var id = 0;
                if (int.TryParse(search.Uri.Substring(search.Uri.TrimEnd('/').LastIndexOf("/") + 1).TrimEnd('/'), out id))
                {
                    var questions = new Questions()
                    {
                        Qid = id,
                        Title = search.Title,
                        Content = search.Content,
                        Award = 0,
                        QuestionUserInfo = new QuestionUserInfo
                        {
                            UserName = search.UserName,
                            IconName = search.UserAlias
                        },
                        DateAdded = search.PublishTime,
                        DiggCount = search.VoteTimes,
                        AnswerCount = search.CommentTimes,
                        ViewCount = search.ViewTimes
                    };

                    var articlesDetails = new QuestionsDetailsPage(questions);

                    await NavigationService.PushAsync(Navigation, articlesDetails);
                }
                this.SearchsListView.SelectedItem = null;
            };
        }
    }
}