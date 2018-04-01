
using Android.App;
using Com.Umeng.Weixin.Callback;

namespace XamCnblogs.Droid.wxapi
{
    [Activity(Name = "com.android.cnblogs.wxapi.WXEntryActivity", Exported = true, Theme = "@style/WXEntryStyle", ConfigurationChanges = Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class WXEntryActivity : WXCallbackActivity
    {
    }
}