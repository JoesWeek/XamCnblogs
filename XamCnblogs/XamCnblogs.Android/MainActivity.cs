
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Com.Tencent.Android.Tpush;
using Com.Umeng.Socialize;
using FFImageLoading.Forms.Droid;
using FormsToolkit.Droid;
using Xam.Plugin.WebView.Droid;
using XamCnblogs.Droid.Helpers;

namespace XamCnblogs.Droid
{
    [Activity(Exported = true,
        LaunchMode = LaunchMode.SingleTask,
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IXGIOperateCallback
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FormsWebViewRenderer.Initialize();
            Toolkit.Init();
            CachedImageRenderer.Init(true);

            Shares.Init(this);

            XGPushConfig.EnableDebug(this, !BuildConfig.Debug);
            XGPushManager.RegisterPush(this, this);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new UI.App());
        }

        public void OnFail(Java.Lang.Object data, int flag, string message)
        {
        }

        public void OnSuccess(Java.Lang.Object data, int flag)
        {
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            UMShareAPI.Get(this).OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}

