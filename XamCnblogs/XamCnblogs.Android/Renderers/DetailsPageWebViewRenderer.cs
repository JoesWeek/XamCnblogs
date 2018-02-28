using Android.OS;
using Android.Webkit;
using System.ComponentModel;
using System.IO;
using Xam.Plugin.WebView.Abstractions;
using Xam.Plugin.WebView.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.Portable.Interfaces;

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
            try
            {
                var chrome = new HtmlWebChromeClient(this);
                Control.SetWebChromeClient(chrome);
            }
            catch (System.Exception)
            {

                throw;
            }
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
                    new Android.OS.Handler().PostDelayed(() =>
                    {
                        try
                        {
                            var newContentHeight = view.ContentHeight;

                            if (newContentHeight == 0 || customWebViewRenderer == null) return;
                            var element = customWebViewRenderer.Element;
                            element.HeightRequest = newContentHeight;
                        }
                        catch (System.Exception ex)
                        {
                            DependencyService.Get<ILog>().SaveLog("HtmlWebChromeClient" , ex);
                        }
                    }, 225);
                }
                base.OnProgressChanged(view, newProgress);
            }

        }

        
    }
}