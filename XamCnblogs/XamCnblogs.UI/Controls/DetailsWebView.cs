using Xam.Plugin.WebView.Abstractions;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.UI.Controls
{
    public class DetailsWebView : FormsWebView
    {
        public static readonly BindableProperty LoadStatusProperty = BindableProperty.Create(nameof(LoadStatus), typeof(LoadMoreStatus), typeof(DetailsWebView), LoadMoreStatus.StausDefault, propertyChanged: OnLoadStatusChanged);

        public LoadMoreStatus LoadStatus
        {
            get { return (LoadMoreStatus)GetValue(LoadStatusProperty); }
            set { SetValue(LoadStatusProperty, value); }
        }
        static void OnLoadStatusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as DetailsWebView).NotifyLoadStatus((LoadMoreStatus)newValue);
        }
        public async void NotifyLoadStatus(LoadMoreStatus loadStatus)
        {
            await this.InjectJavascriptAsync("updateLoadStatus(" + (int)loadStatus + ");");
        }
    }
}
