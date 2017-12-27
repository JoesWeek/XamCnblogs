
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using FormsToolkit.Droid;
using Xam.Plugin.WebView.Droid;

namespace XamCnblogs.Droid
{
    [Activity(Label = "XamCnblogs",
        Exported = true,
        Icon = "@drawable/ic_launcher",
        LaunchMode = LaunchMode.SingleTask,
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

            Com.Chteam.Agent.BuglyAgentHelper.CheckUpgrade();

            LoadApplication(new UI.App());

        }
    }
}

