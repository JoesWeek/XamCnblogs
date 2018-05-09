using FormsToolkit;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Extensions;
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
using XamCnblogs.UI.Pages.New;

namespace XamCnblogs.UI.Pages.Status
{
	public partial class StatusesDetailsPage : ContentPage
    {
        StatusesDetailsViewModel ViewModel => vm ?? (vm = BindingContext as StatusesDetailsViewModel);
        StatusesDetailsViewModel vm;
        Statuses statuses;

        public StatusesDetailsPage(Statuses statuses)
        {
            this.statuses = statuses;
            InitializeComponent();
            Title = statuses.UserDisplayName + "的闪存";
            BindingContext = new StatusesDetailsViewModel(statuses);

            formsWebView.OnContentLoaded += delegate (object sender, EventArgs e)
            {
                RefreshStatuses();
            };

            formsWebView.AddLocalCallback("reload", async delegate (string obj)
            {
                if (formsWebView.LoadStatus == LoadMoreStatus.StausDefault || formsWebView.LoadStatus == LoadMoreStatus.StausError || formsWebView.LoadStatus == LoadMoreStatus.StausFail)
                {
                    var questionsComments = JsonConvert.SerializeObject(await ViewModel.ReloadCommentsAsync());
                    await formsWebView.InjectJavascriptAsync("updateComments(" + questionsComments + ");");
                }
            });
            formsWebView.AddLocalCallback("editItem", delegate (string id)
            {
                var statusesComments = ViewModel.StatusesComments.Where(n => n.Id == int.Parse(id)).FirstOrDefault();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var page = new StatusesCommentPopupPage(statuses, new Action<StatusesComments>(OnResult));
                    await Navigation.PushPopupAsync(page);
                });
            });
            formsWebView.AddLocalCallback("deleteItem", async delegate (string id)
            {
                var result = await ViewModel.DeleteStatusesCommentsAsync(int.Parse(id));
                await formsWebView.InjectJavascriptAsync("deleteComment(" + id + "," + result.ToString().ToLower() + ");");
            });
        }
        async void RefreshStatuses()
        {
            var model = JsonConvert.SerializeObject(statuses);
            await formsWebView.InjectJavascriptAsync("updateModel(" + model + ");");
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
                var page = new StatusesCommentPopupPage(statuses, new Action<StatusesComments>(OnResult));
                if (page != null && Navigation != null)
                    await Navigation.PushPopupAsync(page);
            }
        }
        async void OnResult(StatusesComments result)
        {
            if (result != null)
            {
                ViewModel.AddComment(result);
                await formsWebView.InjectJavascriptAsync("updateComment(" + JsonConvert.SerializeObject(result) + ");");
            }
        }
    }
}