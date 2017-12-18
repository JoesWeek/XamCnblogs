using System;

using Xamarin.Forms;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.KbArticle
{
    public partial class KbArticlesDetailsPage : ContentPage
    {
        KbArticlesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as KbArticlesDetailsViewModel);
        KbArticlesDetailsViewModel vm;
        public KbArticlesDetailsPage(KbArticles kbArticles)
        {
            InitializeComponent();
            BindingContext = new KbArticlesDetailsViewModel(kbArticles);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.RefreshCommand.Execute(null);
        }
        void OnTapped(object sender, EventArgs args)
        {
            ViewModel.RefreshCommand.Execute(null);
        }
    }
}