using Rg.Plugins.Popup.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Question
{
    public partial class QuestionsEditPage : ContentPage
    {
        QuestionsViewModel ViewModel => vm ?? (vm = BindingContext as QuestionsViewModel);
        QuestionsViewModel vm;
        Questions questions;
        ActivityIndicatorPopupPage popupPage;
        Action<Questions> result;
        public QuestionsEditPage(Questions questions) : base()
        {
            Init(questions);
        }
        public QuestionsEditPage(Questions questions, Action<Questions> result) : base()
        {
            this.result = result;
            Init(questions);
        }
        void Init(Questions questions)
        {
            this.questions = questions;
            InitializeComponent();
            BindingContext = new QuestionsViewModel();
            if (questions.Qid > 0)
            {
                Title = "编辑问题";
            }
            else
            {
                Title = "提问";
            }
            var cancel = new ToolbarItem
            {
                Text = "保存",
                Command = new Command(async () =>
                {
                    await ExecuteQuestionkEditAsync();
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "menu_send.png";

            this.editorContent.Text = questions.Content;
            this.entryTitle.Text = questions.Title;
            this.entryTags.Text = questions.TagsDisplay;
        }
        public async Task ExecuteQuestionkEditAsync()
        {
            var toast = DependencyService.Get<IToast>();
            var title = this.entryTitle.Text;
            var tags = this.entryTags.Text;
            var content = this.editorContent.Text;
            if (title == null)
            {
                toast.SendToast("请输入标题");
            }
            else if (title.Length < 3)
            {
                toast.SendToast("标题最少要3个字.");
            }
            else
            {
                if (AboutSettings.Current.WeibaToggled && questions.Qid == 0)
                    content += "<br/>" + AboutSettings.Current.WeibaContent;

                questions.Title = title;
                questions.Tags = tags;
                questions.Summary = content;
                questions.Content = content;
                questions.DateAdded = DateTime.Now;
                questions.QuestionUserInfo = new QuestionUserInfo()
                {
                    UserID = UserSettings.Current.SpaceUserId,
                    UserName = UserSettings.Current.DisplayName,
                    IconName = UserSettings.Current.Avatar
                };
                if (popupPage == null)
                {
                    popupPage = new ActivityIndicatorPopupPage();
                }
                await Navigation.PushPopupAsync(popupPage);

                if (await ViewModel.ExecuteQuestionsEditCommandAsync(questions))
                {
                    await Navigation.RemovePopupPageAsync(popupPage);
                    if (result != null)
                        result.Invoke(questions);
                    Navigation.RemovePage(this);
                }
                else
                {
                    await Navigation.RemovePopupPageAsync(popupPage);
                }
            }
        }
    }
}