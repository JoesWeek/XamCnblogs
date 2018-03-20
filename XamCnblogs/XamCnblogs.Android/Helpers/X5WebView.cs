using Android.Content;
using Android.Util;
using Android.Widget;
using Com.Tencent.Smtt.Sdk;
using System;

namespace XamCnblogs.Droid.Helpers
{
    public class X5WebView : WebView
    {
        public X5WebView(Context context, IAttributeSet attr) : base(context, attr)
        {
            this.WebViewClient = new X5WebViewClient();
            // this.setWebChromeClient(chromeClient);
            // WebStorage webStorage = WebStorage.getInstance();
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
            // webSetting.setLoadWithOverviewMode(true);
            webSetting.SetAppCacheEnabled(true);
            // webSetting.setDatabaseEnabled(true);
            webSetting.DomStorageEnabled = true;
            webSetting.SetGeolocationEnabled(true);
            webSetting.SetAppCacheMaxSize(long.MaxValue);
            // webSetting.setPageCacheCapacity(IX5WebSettings.DEFAULT_CACHE_CAPACITY);
            webSetting.SetPluginState(WebSettings.PluginState.OnDemand);
            // webSetting.setRenderPriority(WebSettings.RenderPriority.HIGH);
            webSetting.CacheMode = WebSettings.LoadNoCache;
            // this.getSettingsExtension().setPageCacheCapacity(IX5WebSettings.DEFAULT_CACHE_CAPACITY);//extension
            // settings 的设计
        }
        //protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        //{
        //    bool ret = base.DrawChild(canvas, child, drawingTime);
        //    canvas.Save();
        //    Paint paint = new Paint();
        //    paint.Color = Color.ParseColor("0x7fff0000");
        //    paint.TextSize = 24F;
        //    paint.AntiAlias = true;

        //    if (GetX5WebViewExtension() != null)
        //    {
        //        canvas.DrawText(this.Context.PackageName + "-pid:"
        //                + Android.OS.Process.MyPid(), 10, 50, paint);
        //        canvas.DrawText(
        //                "X5  Core:" + QbSdk.GetTbsVersion(this.Context), 10,
        //                100, paint);
        //    }
        //    else
        //    {
        //        canvas.DrawText(this.Context.PackageName+ "-pid:"
        //                + Android.OS.Process.MyPid(), 10, 50, paint);
        //        canvas.DrawText("Sys Core", 10, 100, paint);
        //    }
        //    canvas.DrawText(Build.Manufacturer, 10, 150, paint);
        //    canvas.DrawText(Build.Model, 10, 200, paint);
        //    canvas.Restore();
        //    return ret;
        //}
    }
    public class X5WebViewClient : WebViewClient
    {
        public new bool ShouldOverrideUrlLoading(WebView view, String url)
        {
            view.LoadUrl(url);
            return true;
        }
    }
    public interface WebViewJavaScriptFunction
    {
        void OnJsFunctionCalled(string tag);
    }

}