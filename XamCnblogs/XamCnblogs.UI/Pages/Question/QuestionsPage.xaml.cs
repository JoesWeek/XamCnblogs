using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Question
{
    public partial class QuestionsPage : ContentPage
    {
        QuestionsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsViewModel);
        QuestionsViewModel vm;
        public QuestionsPage(int position = 0) : base()
        {
            InitializeComponent();
            BindingContext = new QuestionsViewModel(position);
            this.QuestionsListView.ItemSelected += async delegate
            {
                var questions = QuestionsListView.SelectedItem as Questions;
                if (questions == null)
                    return;

                var questionsDetails = new QuestionsDetailsPage(questions);

                await NavigationService.PushAsync(Navigation, questionsDetails);
                this.QuestionsListView.SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdatePage();
        }
        private void UpdatePage()
        {
            bool forceRefresh = (DateTime.Now > (ViewModel?.NextRefreshTime ?? DateTime.Now));

            if (forceRefresh)
            {
                //刷新
                ViewModel.RefreshCommand.Execute(null);
            }
            else
            {
                //加载本地数据
                if (ViewModel.Questions.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
    }
}