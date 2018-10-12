using FormsToolkit;
using System;
using System.Linq;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Controls;

namespace XamCnblogs.UI.Pages.Status {
    public partial class StatusesPage : ContentPage {
        StatusesViewModel ViewModel => vm ?? (vm = BindingContext as StatusesViewModel);
        StatusesViewModel vm;
        bool hasInitialization;
        int position = 0;
        public StatusesPage(int position = 0) : base() {
            this.position = position;
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);

        }

        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                BindingContext = new StatusesViewModel(position);

                var cancel = new ToolbarItem {
                    Text = "添加",
                    Command = new Command(async () => {
                        if (UserTokenSettings.Current.HasExpiresIn()) {
                            MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                        }
                        else {
                            await NavigationService.PushAsync(Navigation, new StatusesEditPage(new Statuses(), new Action<Statuses>(OnResult)));
                        }
                    }),
                    Icon = "toolbar_add.png"
                };
                ToolbarItems.Add(cancel);

                this.StatusesListView.HasFloatingView = true;
                this.StatusesListView.ItemSelected += async delegate {
                    var statuses = StatusesListView.SelectedItem as Statuses;
                    this.StatusesListView.SelectedItem = null;
                    if (statuses == null)
                        return;

                    var statusesDetails = new StatusesDetailsPage(statuses);
                    if (statuses.Id > 0)
                        await NavigationService.PushAsync(Navigation, statusesDetails);
                };

                var floatingView = new FloatingView();

                this.StatusesListView.FloatingChanged += delegate (object sender, bool floating) {
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
                            await NavigationService.PushAsync(this.Navigation, new StatusesEditPage(new Statuses()));
                        }
                    };
                    this.AbsoluteLayout.Children.Add(floatingView);
                }

                ViewModel.GetClientStatusesAsync();
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
        private void OnResult(Statuses result) {
            if (result != null) {
                ViewModel.EditStatuses(result);
                StatusesListView.ScrollTo(ViewModel.Statuses.FirstOrDefault(), ScrollToPosition.Start, false);
            }
        }
    }
}