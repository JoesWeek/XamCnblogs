
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Com.Umeng.Socialize;
using FFImageLoading.Forms.Droid;
using FormsToolkit.Droid;
using Xam.Plugin.WebView.Droid;
using XamCnblogs.Droid.Helpers;

namespace XamCnblogs.Droid
{
    [Activity(Label = "@string/AppName",
        Exported = true,
        Icon = "@drawable/ic_launcher",
        LaunchMode = LaunchMode.SingleTask,
        Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            FormsWebViewRenderer.Initialize();

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Toolkit.Init();
            CachedImageRenderer.Init(true);

            Shares.Init(this);

            LoadApplication(new UI.App());

        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            UMShareAPI.Get(this).OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}

