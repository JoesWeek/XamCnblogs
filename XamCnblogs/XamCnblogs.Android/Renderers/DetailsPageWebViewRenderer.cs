using Android.OS;
using Android.Webkit;
using System.ComponentModel;
using System.IO;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.DetailsPageWebView), typeof(DetailsPageWebViewRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class DetailsPageWebViewRenderer : FormsWebViewRenderer
    {
        public DetailsPageWebViewRenderer()
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<FormsWebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.Settings.JavaScriptEnabled = true;

                if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                    Control.Settings.UseWideViewPort = true;
                
                LoadData();
            }
        }
        private void LoadData()
        {
            var chrome = new HtmlWebChromeClient(this);
            Control.SetWebChromeClient(chrome);
        }
        
        public class HtmlWebChromeClient : WebChromeClient
        {
            DetailsPageWebViewRenderer customWebViewRenderer;
            internal HtmlWebChromeClient(DetailsPageWebViewRenderer customWebViewRenderer)
            {
                this.customWebViewRenderer = customWebViewRenderer;
            }
            public override void OnProgressChanged(global::Android.Webkit.WebView view, int newProgress)
            {
                if (newProgress == 100)
                {
                    new Handler().PostDelayed(() =>
                    {
                        var newContentHeight = view.ContentHeight;

                        if (newContentHeight == 0) return;
                        var element = customWebViewRenderer.Element;
                        element.HeightRequest = newContentHeight;
                    }, 300);
                }
                base.OnProgressChanged(view, newProgress);
            }

        }
    }
}