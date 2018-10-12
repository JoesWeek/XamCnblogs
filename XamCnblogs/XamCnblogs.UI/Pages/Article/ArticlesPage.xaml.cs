using System;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.Article
{
    public partial class ArticlesPage : ContentPage
    {
        ArticlesViewModel ViewModel => vm ?? (vm = BindingContext as ArticlesViewModel);
        ArticlesViewModel vm;
        bool hasInitialization;
        int position = 0;
        public ArticlesPage(int position = 0) : base()
        {
            this.position = position;
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            if (!hasInitialization) {

                BindingContext = new ArticlesViewModel(position);

                this.ArticlesListView.ItemSelected += async delegate
                {
                    var articles = ArticlesListView.SelectedItem as Articles;
                    this.ArticlesListView.SelectedItem = null;
                    if (articles == null)
                        return;

                    var articlesDetails = new ArticlesDetailsPage(articles);

                    await NavigationService.PushAsync(Navigation, articlesDetails);
                };

                ViewModel.GetClientArticlesAsync();

                hasInitialization = true;
            }
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
    }
}