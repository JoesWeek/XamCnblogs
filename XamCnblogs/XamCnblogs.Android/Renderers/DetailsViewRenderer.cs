using Android.Content;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Tencent.Smtt.Sdk;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(DetailsView), typeof(DetailsViewRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class DetailsViewRenderer : ViewRenderer<DetailsView, Com.Tencent.Smtt.Sdk.WebView>
    {
        bool _disposed;

        public static string MimeType = "text/html";

        public static string EncodingType = "UTF-8";

        public static string BaseUrl { get; set; } = "file:///android_asset/";

        public DetailsViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DetailsView> e)
        {
            base.OnElementChanged(e);

            if (Control == null && Element != null)
                SetupControl();

            if (e.NewElement != null)
                SetSource();
        }

        void SetupControl()
        {
            var webView = new X5WebView(this.Context, null);

            webView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
            webView.HorizontalScrollBarEnabled = false;
            webView.VerticalScrollBarEnabled = false;
            webView.WebViewClient = new X5WebViewClient(this);

            SetNativeControl(webView);
        }

        internal void SetSource()
        {
            if (Element == null || Control == null || string.IsNullOrWhiteSpace(Element.Source)) return;

            Control.LoadDataWithBaseURL(BaseUrl, Element.Source, MimeType, EncodingType, "");
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Source")
            {
                SetSource();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                if (Control != null)
                    Control.Destroy();
            }
            base.Dispose(disposing);
        }
        class X5WebView : Com.Tencent.Smtt.Sdk.WebView
        {
            public X5WebView(Context context, IAttributeSet attr) : base(context, attr)
            {
                InitWebViewSettings();
                this.View.Clickable = true;
            }
            private void InitWebViewSettings()
            {
                WebSettings webSetting = this.Settings;
                webSetting.JavaScriptEnabled = true;
                webSetting.JavaScriptCanOpenWindowsAutomatically = true;
                webSetting.AllowFileAccess = true;
                webSetting.SetLayoutAlgorithm(WebSettings.LayoutAlgorithm.NarrowColumns);
                webSetting.SetSupportZoom(true);
                webSetting.BuiltInZoomControls = true;
                webSetting.DisplayZoomControls = false;
                webSetting.UseWideViewPort = true;
                webSetting.SetSupportMultipleWindows(true);
                webSetting.SetAppCacheEnabled(true);
                webSetting.DomStorageEnabled = true;
                webSetting.SetGeolocationEnabled(true);
                webSetting.SetAppCacheMaxSize(long.MaxValue);
                webSetting.SetPluginState(WebSettings.PluginState.OnDemand);
                webSetting.CacheMode = WebSettings.LoadCacheElseNetwork;
            }
        }
        class X5WebViewClient : WebViewClient
        {
            readonly WeakReference<DetailsViewRenderer> Reference;

            public X5WebViewClient(DetailsViewRenderer renderer)
            {
                Reference = new WeakReference<DetailsViewRenderer>(renderer);
            }
            public async override void OnPageFinished(Com.Tencent.Smtt.Sdk.WebView view, string url)
            {
                if (Reference == null || !Reference.TryGetTarget(out DetailsViewRenderer renderer)) return;
                if (renderer.Element == null) return;

                int i = 10;
                while (view.ContentHeight == 0 && i-- > 0)
                    await System.Threading.Tasks.Task.Delay(200);
                renderer.Element.HeightRequest = view.ContentHeight;
                await System.Threading.Tasks.Task.Delay(200);
                renderer.Element.OnCompleted();
            }
        }
    }
}