﻿
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Com.Tencent.Android.Tpush;
using Com.Umeng.Socialize;
using FFImageLoading.Forms.Droid;
using FormsToolkit.Droid;
using XamCnblogs.Droid.Helpers;

namespace XamCnblogs.Droid
{
    [Activity(Label = "@string/AppName",
        Exported = true,
        Icon = "@drawable/ic_launcher",
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

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Toolkit.Init();
            CachedImageRenderer.Init(true);

            Shares.Init(this);

            XGPushConfig.EnableDebug(this, !BuildConfig.Debug);
            XGPushManager.RegisterPush(this, this);
            var str = XGPushConfig.GetToken(this);

            LoadApplication(new UI.App());
        }
        public void OnFail(Java.Lang.Object data, int flag, string message)
        {
            Log.Error("TPush", "注册识别，设备token为：" + data);
        }

        public void OnSuccess(Java.Lang.Object data, int flag)
        {
            Log.Debug("TPush", "注册成功，设备token为：" + data);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            UMShareAPI.Get(this).OnActivityResult(requestCode, (int)resultCode, data);
        }
    }
}

