using System;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.KbArticle {
    public partial class KbArticlesPage : ContentPage {
        KbArticlesViewModel ViewModel => vm ?? (vm = BindingContext as KbArticlesViewModel);
        KbArticlesViewModel vm;
        bool hasInitialization;
        public KbArticlesPage() {
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {
                BindingContext = new KbArticlesViewModel();

                Title = "知识库";

                this.KbArticlesListView.ItemSelected += async delegate {
                    var kbarticles = KbArticlesListView.SelectedItem as KbArticles;
                    this.KbArticlesListView.SelectedItem = null;
                    if (kbarticles == null)
                        return;

                    var kbarticlesDetails = new KbArticlesDetailsPage(kbarticles);

                    await NavigationService.PushAsync(Navigation, kbarticlesDetails);

                };
                ViewModel.GetClientKbArticlesAsync();

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
    }
}