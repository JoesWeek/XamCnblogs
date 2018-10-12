using FormsToolkit;
using System;
using System.Linq;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Controls;

namespace XamCnblogs.UI.Pages.Question {
    public partial class QuestionsPage : ContentPage {
        QuestionsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsViewModel);
        QuestionsViewModel vm;
        bool hasInitialization;
        int position = 0;
        public QuestionsPage(int position = 0) : base() {
            this.position = position;
            InitializeComponent();

            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);            
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                BindingContext = new QuestionsViewModel(position);

                var cancel = new ToolbarItem {
                    Text = "添加",
                    Command = new Command(async () => {
                        if (UserTokenSettings.Current.HasExpiresIn()) {
                            MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                        }
                        else {
                            await NavigationService.PushAsync(Navigation, new QuestionsEditPage(new Questions(), new Action<Questions>(OnResult)));
                        }
                    }),
                    Icon = "toolbar_add.png"
                };
                ToolbarItems.Add(cancel);

                this.QuestionsListView.HasFloatingView = true;

                this.QuestionsListView.ItemSelected += async delegate {
                    var questions = QuestionsListView.SelectedItem as Questions;
                    this.QuestionsListView.SelectedItem = null;
                    if (questions == null)
                        return;

                    var questionsDetails = new QuestionsDetailsPage(questions);

                    await NavigationService.PushAsync(Navigation, questionsDetails);
                };

                var floatingView = new FloatingView();

                this.QuestionsListView.FloatingChanged += delegate (object sender, bool floating) {
                    if (Device.RuntimePlatform == Device.Android) {
                        if (floating) {
                            floatingView.ToggleFloatingView = true;
                        }
                        else {
                            floatingView.ToggleFloatingView = false;
                        }
                    }
                };
                if (Device.RuntimePlatform == Device.Android) {
                    floatingView.Image = "toolbar_add.png";
                    floatingView.ButtonColor = Color.FromHex("#E64A19");
                    AbsoluteLayout.SetLayoutBounds(floatingView, new Rectangle(1, .9, 80, 90));
                    AbsoluteLayout.SetLayoutFlags(floatingView, AbsoluteLayoutFlags.PositionProportional);
                    floatingView.Clicked += async delegate (object sender, EventArgs e) {

                        if (UserTokenSettings.Current.HasExpiresIn()) {
                            MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                        }
                        else {
                            await NavigationService.PushAsync(this.Navigation, new QuestionsEditPage(new Questions()));
                        }
                    };

                    this.AbsoluteLayout.Children.Add(floatingView);
                }
                ViewModel.GetClientQuestionsAsync();
                hasInitialization = true;
            }
            UpdatePage();
        }
        private void UpdatePage() {
            bool forceRefresh = (DateTime.Now > (ViewModel?.NextRefreshTime ?? DateTime.Now));

            if (forceRefresh) {
                //刷新
                ViewModel.RefreshCommand.Execute(null);
            }
        }

        private void OnResult(Questions result) {
            if (result != null) {
                ViewModel.EditQuestions(result);
                QuestionsListView.ScrollTo(ViewModel.Questions.FirstOrDefault(), ScrollToPosition.Start, false);
            }
        }
    }
}