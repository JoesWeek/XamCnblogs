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
            this.StatusesListView.ItemSelected += async delegate
            {
                var statuses = StatusesListView.SelectedItem as Statuses;
                if (statuses == null)
                    return;

                var statusesDetails = new StatusesDetailsPage(statuses);

                await NavigationService.PushAsync(Navigation, statusesDetails);
                this.StatusesListView.SelectedItem = null;
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdatePage();
        }
        public void OnResume()
        {
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
                if (ViewModel.Statuses.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
    }
}