using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Tencent.Smtt.Sdk;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(AuthorizeView), typeof(AuthorizeViewRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class AuthorizeViewRenderer : ViewRenderer
    {
        bool _disposed;
        private X5WebView webView;
        public AuthorizeViewRenderer(Context context) : base(context)
        {
            var cookieSyncManager = Com.Tencent.Smtt.Sdk.CookieManager.Instance;

            cookieSyncManager.SetAcceptCookie(true);
            cookieSyncManager.RemoveSessionCookie();
            cookieSyncManager.RemoveAllCookie();
        }
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;
            var view = Inflate(this.Context, Resource.Layout.authorizeview, null);
            var viewParent = (ViewGroup)view.FindViewById(Resource.Id.webView1);

            webView = new X5WebView(this.Context, null);
            webView.WebViewClient = new X5WebViewClient(this);

            viewParent.AddView(webView, new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent));

            var progressBar = (Android.Widget.ProgressBar)view.FindViewById(Resource.Id.progressBar1);
            progressBar.Max = 100;
            progressBar.ProgressDrawable = Context.GetDrawable(Resource.Drawable.color_progressbar);

            webView.WebChromeClient = new X5WebChromeClient(this, progressBar);

            webView.LoadUrl((Element as XamCnblogs.UI.Controls.AuthorizeView).Source);


            SetNativeControl(view);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                if (webView != null)
                    webView.Destroy();
            }
            base.Dispose(disposing);
        }
        public class X5WebView : Com.Tencent.Smtt.Sdk.WebView
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
                webSetting.CacheMode = WebSettings.LoadNoCache;
            }
        }
        public class X5WebViewClient : WebViewClient
        {
            private AuthorizeViewRenderer renderer;
            public X5WebViewClient(AuthorizeViewRenderer renderer)
            {
                this.renderer = renderer;
            }
            public override bool ShouldOverrideUrlLoading(Com.Tencent.Smtt.Sdk.WebView view, string url)
            {
                if (url.IndexOf("https://oauth.cnblogs.com/auth/callback#code=") > -1)
                {
                    var codeindex = url.IndexOf("#code=") + 6;
                    var tokenindex = url.IndexOf("&id_token=");
                    var code = url.Substring(codeindex, tokenindex - codeindex);
                    if (code != "")
                    {
                        (renderer.Element as AuthorizeView).OnAuthorizeStarted(new AuthorizeStartedEventArgs
                        {
                            Code = code
                        });
                    }
                    view.StopLoading();
                    return true;
                }
                else
                {
                    view.LoadUrl(url);
                    return true;
                }
            }
        }
        private class X5WebChromeClient : Com.Tencent.Smtt.Sdk.WebChromeClient
        {
            private AuthorizeViewRenderer renderer;
            private Android.Widget.ProgressBar progressBar;
            public X5WebChromeClient(AuthorizeViewRenderer renderer, Android.Widget.ProgressBar progressBar)
            {
                this.renderer = renderer;
                this.progressBar = progressBar;
            }
            public override void OnProgressChanged(Com.Tencent.Smtt.Sdk.WebView view, int newProgress)
            {
                progressBar.Progress = newProgress;
                if (newProgress < 100)
                {
                    if (progressBar.Visibility == ViewStates.Gone)
                        progressBar.Visibility = ViewStates.Visible;
                }
                else
                {
                    progressBar.Visibility = ViewStates.Gone;
                }
                base.OnProgressChanged(view, newProgress);
            }
        }

    }
}