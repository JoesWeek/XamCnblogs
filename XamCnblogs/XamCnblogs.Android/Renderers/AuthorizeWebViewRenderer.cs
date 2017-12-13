using Android.Webkit;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.AuthorizeWebView), typeof(AuthorizeWebViewRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class AuthorizeWebViewRenderer : FormsWebViewRenderer
    {
        public AuthorizeWebViewRenderer()
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<FormsWebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                CookieSyncManager cookieSyncManager = CookieSyncManager.CreateInstance(Control.Context);
                CookieManager cookieManager = CookieManager.Instance;
                cookieManager.SetAcceptCookie(true);
                cookieManager.RemoveSessionCookie();
                cookieManager.RemoveAllCookie();

            }
        }
    }
}