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
using XamCnblogs.UI.Pages.Article;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class ArticlesPage : ContentPage
    {
        BlogsViewModel ViewModel => vm ?? (vm = BindingContext as BlogsViewModel);
        BlogsViewModel vm;
        bool hasInitialization;
        string blogApp;
        public ArticlesPage(string blogApp) : base()
        {
            this.blogApp = blogApp;
            InitializeComponent();
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!hasInitialization) {

                BindingContext = new BlogsViewModel(blogApp);
                this.ArticlesListView.ItemSelected += async delegate
                {
                    var articles = ArticlesListView.SelectedItem as Articles;
                    this.ArticlesListView.SelectedItem = null;
                    if (articles == null)
                        return;

                    var articlesDetails = new ArticlesDetailsPage(articles);

                    await NavigationService.PushAsync(Navigation, articlesDetails);
                };
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
            else
            {
                //加载本地数据
                if (ViewModel.Articles.Count == 0)
                    ViewModel.RefreshCommand.Execute(null);
            }
        }
    }
}