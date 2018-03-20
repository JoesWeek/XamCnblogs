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
        public ArticlesPage(int position = 0) : base()
        {
            InitializeComponent();
            BindingContext = new ArticlesViewModel(position);
            this.ArticlesListView.ItemSelected += async delegate
            {
                var articles = ArticlesListView.SelectedItem as Articles;
                if (articles == null)
                    return;

                var articlesDetails = new ArticlesDetailsPage(articles);

                await NavigationService.PushAsync(Navigation, articlesDetails);
                this.ArticlesListView.SelectedItem = null;
            };

            ViewModel.GetClientArticlesAsync();
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
    }
}