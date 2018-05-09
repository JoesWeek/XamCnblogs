using FormsToolkit;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Account;
using XamCnblogs.UI.Pages.New;

namespace XamCnblogs.UI.Pages.Question
{
    public partial class QuestionsDetailsPage : ContentPage
    {
        QuestionsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsDetailsViewModel);
        QuestionsDetailsViewModel vm;
        Questions questions;
        public QuestionsDetailsPage(Questions questions)
        {
            this.questions = questions;
            InitializeComponent();
            BindingContext = new QuestionsDetailsViewModel(questions);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().Shares("https://q.cnblogs.com/q/" + questions.Qid + "/", questions.Title);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";

            formsWebView.OnContentLoaded += delegate (object sender, EventArgs e)
            {
                RefreshQuestions();
            };

            formsWebView.AddLocalCallback("reload", async delegate (string obj)
            {
                if (formsWebView.LoadStatus == LoadMoreStatus.StausDefault || formsWebView.LoadStatus == LoadMoreStatus.StausError || formsWebView.LoadStatus == LoadMoreStatus.StausFail)
                {
                    var questionsComments = JsonConvert.SerializeObject(await ViewModel.ReloadAnswersAsync());
                    await formsWebView.InjectJavascriptAsync("updateComments(" + questionsComments + ");");
                }
            });
            formsWebView.AddLocalCallback("editItem", delegate (string id)
            {
                var questionsAnswers = ViewModel.QuestionsAnswers.Where(n => n.AnswerID == int.Parse(id)).FirstOrDefault();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = new QuestionsAnswersPopupPage(questions, new Action<QuestionsAnswers>(OnResult), questionsAnswers);
                    await Navigation.PushPopupAsync(page);
                });
            });
            formsWebView.AddLocalCallback("deleteItem", async delegate (string id)
            {
                var result = await ViewModel.DeleteQuestionAnswersAsync(int.Parse(id));
                await formsWebView.InjectJavascriptAsync("deleteComment(" + id + "," + result.ToString().ToLower() + ");");
            });
            formsWebView.AddLocalCallback("itemSelected", delegate (string id)
           {
               var questionsAnswers = ViewModel.QuestionsAnswers.Where(n => n.AnswerID == int.Parse(id)).FirstOrDefault();
               Device.BeginInvokeOnMainThread(async () =>
               {
                   var answersDetails = new AnswersDetailsPage(questionsAnswers);
                   if (questionsAnswers.AnswerID > 0)
                       await NavigationService.PushAsync(Navigation, answersDetails);
               });
           });
        }

        async void RefreshQuestions()
        {
            var question = JsonConvert.SerializeObject(await ViewModel.RefreshQuestionsAsync());
            await formsWebView.InjectJavascriptAsync("updateModel(" + question + ");");
        }
        void OnReloadQuestions(object sender, EventArgs args)
        {
            RefreshQuestions();
        }
        async void OnResult(QuestionsAnswers result)
        {
            if (result != null)
            {
                ViewModel.EditComment(result);
                await formsWebView.InjectJavascriptAsync("updateComment(" + JsonConvert.SerializeObject(result) + ");");
            }
        }
        void OnScrollComment(object sender, EventArgs args)
        {
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new QuestionsAnswersPopupPage(questions, new Action<QuestionsAnswers>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var url = "https://q.cnblogs.com/q/" + questions.Qid + "/";
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = questions.Title, LinkUrl = url, FromCNBlogs = true }));
            }
        }
    }
}