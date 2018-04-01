using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Com.Tencent.Android.Tpush;
using Com.Umeng.Socialize;
using Plugin.CurrentActivity;
using System;

namespace XamCnblogs.Droid
{
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            RegisterActivityLifecycleCallbacks(this);

            Com.Chteam.Agent.BuglyAgentHelper.Init(this.ApplicationContext, "e2fab3d122");

            PlatformConfig.SetWeixin("wxcf8f642a8aa4c630", "76ebc29b4194164aee32eedff2e17900");
            PlatformConfig.SetSinaWeibo("1422675167", "02975c36afd93d3ae983f8da9e596b86", "https://api.weibo.com/oauth2/default.html");

        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

    }
}