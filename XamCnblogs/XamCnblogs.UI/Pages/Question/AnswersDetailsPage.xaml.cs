using FormsToolkit;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.New;

namespace XamCnblogs.UI.Pages.Question
{
    public partial class AnswersDetailsPage : ContentPage
    {
        AnswersDetailsViewModel ViewModel => vm ?? (vm = BindingContext as AnswersDetailsViewModel);
        AnswersDetailsViewModel vm;
        QuestionsAnswers answers;

        public AnswersDetailsPage(QuestionsAnswers answers)
        {
            this.answers = answers;
            InitializeComponent();
            BindingContext = new AnswersDetailsViewModel(answers);
            
            formsWebView.OnContentLoaded += delegate (object sender, EventArgs e)
            {
                RefreshAnswers();
            };

            formsWebView.AddLocalCallback("reload", async delegate (string obj)
            {
                if (formsWebView.LoadStatus == LoadMoreStatus.StausDefault || formsWebView.LoadStatus == LoadMoreStatus.StausError || formsWebView.LoadStatus == LoadMoreStatus.StausFail)
                {
                    var questionsComments = JsonConvert.SerializeObject(await ViewModel.ReloadCommentsAsync());
                    await formsWebView.InjectJavascriptAsync("updateComments(" + questionsComments + ");");
                }
            });
            formsWebView.AddLocalCallback("editItem", delegate (string id)
            {
                var questionsAnswers = ViewModel.AnswersComments.Where(n => n.CommentID == int.Parse(id)).FirstOrDefault();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = new AnswersCommentPopupPage(answers, new Action<AnswersComments>(OnResult), questionsAnswers);
                    await Navigation.PushPopupAsync(page);
                });
            });
            formsWebView.AddLocalCallback("deleteItem", async delegate (string id)
            {
                var result = await ViewModel.DeleteAnswersCommentsAsync(int.Parse(id));
                await formsWebView.InjectJavascriptAsync("deleteComment(" + id + "," + result.ToString().ToLower() + ");");
            });
        }
        async void RefreshAnswers()
        {
            var answer = JsonConvert.SerializeObject(answers);
            await formsWebView.InjectJavascriptAsync("updateModel(" + answer + ");");
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
                var page = new AnswersCommentPopupPage(answers, new Action<AnswersComments>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        async void OnResult(AnswersComments result)
        {
            if (result != null)
            {
                ViewModel.EditComment(result);
                await formsWebView.InjectJavascriptAsync("updateComment(" + JsonConvert.SerializeObject(result) + ");");
            }
        }
    }
}