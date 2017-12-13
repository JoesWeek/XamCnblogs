using FormsToolkit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamCnblogs.Portable.Model;
using XamCnblogs.UI.Controls;
using XamCnblogs.UI.Pages.Article;

namespace XamCnblogs.UI.Pages.Android
{
    public class RootPage : MasterDetailPage
    {
        Dictionary<int, XamNavigationPage> pages;
        DeepLinkPage page;
        bool isRunning = false;
        public RootPage()
        {
            pages = new Dictionary<int, XamNavigationPage>();
            Master = new MenuPage(this);

            pages.Add(0, new XamNavigationPage(new ArticlesTopTabbedPage()));

            Detail = pages[0];

            MessagingService.Current.Subscribe<DeepLinkPage>("DeepLinkPage", async (m, p) =>
            {
                page = p;

                if (isRunning)
                    await GoToDeepLink();
            });
        }

        public async Task NavigateAsync(int menuId)
        {
            XamNavigationPage newPage = null;
            if (!pages.ContainsKey(menuId))
            {
                switch (menuId)
                {
                    case (int)AppPage.Articles:
                        pages.Add(menuId, new XamNavigationPage(new ArticlesTopTabbedPage()));
                        break;
                }
            }

            if (newPage == null)
                newPage = pages[menuId];

            if (newPage == null)
                return;

            if (Detail == newPage)
            {
                await newPage.Navigation.PopToRootAsync();
            }

            Detail = newPage;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            isRunning = true;

            await GoToDeepLink();

        }
        async Task GoToDeepLink()
        {
            if (page == null)
                return;
            var p = page.Page;
            var id = page.Id;
            page = null;
            switch (p)
            {
                //case AppPage.Sessions:
                //    await NavigateAsync((int)AppPage.Sessions);
                //    break;
            }

        }
    }
}
