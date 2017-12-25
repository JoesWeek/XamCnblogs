using FormsToolkit;
using Rg.Plugins.Popup.Extensions;
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
using XamCnblogs.UI.Pages.New;

namespace XamCnblogs.UI.Pages.Status
{
	public partial class StatusesDetailsPage : ContentPage
    {
        StatusesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as StatusesDetailsViewModel);
        StatusesDetailsViewModel vm;
        Statuses statuses;
        public StatusesDetailsPage(Statuses statuses)
        {
            this.statuses = statuses;
            InitializeComponent();
            Title = statuses.UserDisplayName + "的闪存";
            BindingContext = new StatusesDetailsViewModel(statuses);
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
                ViewModel.RefreshCommand.Execute(null);
            }
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
        void OnScrollComment(object sender, EventArgs args)
        {
            if (ViewModel.Comments.Count > 0)
                StatusesView.ScrollTo(ViewModel.Comments.First(), ScrollToPosition.Start, false);
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new StatusesCommentPopupPage(statuses.Id, new Action<StatusesComments>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        private void OnResult(StatusesComments result)
        {
            if (result != null)
            {
                ViewModel.AddComment(result);
                StatusesView.ScrollTo(ViewModel.Comments.Last(), ScrollToPosition.Start, false);
            }
        }
    }
}