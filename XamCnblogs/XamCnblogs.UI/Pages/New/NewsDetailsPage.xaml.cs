using FormsToolkit;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.ViewModel;
using XamCnblogs.UI.Pages.Account;

namespace XamCnblogs.UI.Pages.New
{
    public partial class NewsDetailsPage : ContentPage
    {
        NewsDetailsViewModel ViewModel => vm ?? (vm = BindingContext as NewsDetailsViewModel);
        NewsDetailsViewModel vm;
        News news;

        public NewsDetailsPage(News news)
        {
            this.news = news;
            InitializeComponent();
            BindingContext = new NewsDetailsViewModel(news);

            var cancel = new ToolbarItem
            {
                Text = "分享",
                Command = new Command(() =>
                {
                    DependencyService.Get<IShares>().SharesIcon("https://news.cnblogs.com/n/" + news.Id + "/", news.Title, news.TopicIcon);
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_share.png";

            formsWebView.OnContentLoaded += delegate (object sender, EventArgs e)
            {
                RefreshNews();
            };
            formsWebView.AddLocalCallback("reload", async delegate (string obj)
            {
                if (formsWebView.LoadStatus == LoadMoreStatus.StausDefault || formsWebView.LoadStatus == LoadMoreStatus.StausError || formsWebView.LoadStatus == LoadMoreStatus.StausFail)
                {
                    var newsComments = JsonConvert.SerializeObject(await ViewModel.ReloadCommentsAsync());
                    await formsWebView.InjectJavascriptAsync("updateComments(" + newsComments + ");");
                }
            });
            formsWebView.AddLocalCallback("editItem", delegate (string id)
           {
               var newsComments = ViewModel.NewsComments.Where(n => n.CommentID == int.Parse(id)).FirstOrDefault();
               Device.BeginInvokeOnMainThread(async () =>
               {
                   var page = new NewsCommentPopupPage(news, new Action<NewsComments>(OnResult), newsComments);
                   await Navigation.PushPopupAsync(page);
               });
           });
            formsWebView.AddLocalCallback("deleteItem", async delegate (string id)
            {
                var result = await ViewModel.DeleteCommentAsync(int.Parse(id));
                await formsWebView.InjectJavascriptAsync("deleteComment(" + id + "," + result.ToString().ToLower() + ");");
            });
        }

        async void RefreshNews()
        {
            var news = JsonConvert.SerializeObject(await ViewModel.RefreshNewsAsync());
            await formsWebView.InjectJavascriptAsync("updateModel(" + news + ");");
        }
        void OnReloadNews(object sender, EventArgs args)
        {
            RefreshNews();
        }
        void OnScrollComment(object sender, EventArgs args)
        {
        }
        async void OnShowComment(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var page = new NewsCommentPopupPage(news, new Action<NewsComments>(OnResult));
                await Navigation.PushPopupAsync(page);
            }
        }
        private async void OnResult(NewsComments result)
        {
            if (result != null)
            {
                ViewModel.EditComment(result);
                await formsWebView.InjectJavascriptAsync("updateComment(" + JsonConvert.SerializeObject(result) + ");");
            }
        }
        async void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                var url = "https://news.cnblogs.com/n/" + news.Id + "/";
                await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks() { Title = news.Title, LinkUrl = url, FromCNBlogs = true }));
            }
        }
    }
}