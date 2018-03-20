using FormsToolkit;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
        }
        void OnScrollComment(object sender, EventArgs args)
        {
            if (ViewModel.AnswersComment.Count > 0)
                QuestionsDetailsView.ScrollTo(ViewModel.AnswersComment.First(), ScrollToPosition.Start, false);
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new AnswersCommentPopupPage(answers, new Action<AnswersComment>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        private void OnResult(AnswersComment result)
        {
            if (result != null)
            {
                ViewModel.EditComment(result);
                QuestionsDetailsView.ScrollTo(ViewModel.AnswersComment.Last(), ScrollToPosition.Start, false);
            }
        }
        public ICommand EditCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    var page = new AnswersCommentPopupPage(answers, new Action<AnswersComment>(OnResult), e as AnswersComment);
                    await Navigation.PushPopupAsync(page);
                });
            }
        }
    }
}