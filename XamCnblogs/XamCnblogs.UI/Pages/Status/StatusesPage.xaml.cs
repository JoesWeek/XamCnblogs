using FormsToolkit;
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
using XamCnblogs.UI.Pages.Question;

namespace XamCnblogs.UI.Pages.Status
{
    public partial class StatusesPage : ContentPage
    {
        StatusesViewModel ViewModel => vm ?? (vm = BindingContext as StatusesViewModel);
        StatusesViewModel vm;
        public StatusesPage(int position = 0) : base()
        {
            InitializeComponent();
            BindingContext = new StatusesViewModel(position);

            var cancel = new ToolbarItem
            {
                Text = "添加",
                Command = new Command(async () =>
                {
                    if (UserTokenSettings.Current.HasExpiresIn())
                    {
                        MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                    }
                    else
                    {
                        await NavigationService.PushAsync(Navigation, new StatusesEditPage(new Statuses(), new Action<Statuses>(OnResult)));
                    }
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_add.png";

            this.StatusesListView.ItemSelected += async delegate
            {
                var statuses = StatusesListView.SelectedItem as Statuses;
                if (statuses == null)
                    return;

                var statusesDetails = new StatusesDetailsPage(statuses);
                if (statuses.Id > 0)
                    await NavigationService.PushAsync(Navigation, statusesDetails);
                this.StatusesListView.SelectedItem = null;
            };
            ViewModel.GetClientStatusesAsync();
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
        }
        private void OnResult(Statuses result)
        {
            if (result != null)
            {
                ViewModel.EditStatuses(result);
                StatusesListView.ScrollTo(ViewModel.Statuses.FirstOrDefault(), ScrollToPosition.Start, false);
            }
        }
    }
}