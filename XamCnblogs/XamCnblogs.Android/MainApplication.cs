using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Com.Umeng.Socialize;
using Plugin.CurrentActivity;

namespace XamCnblogs.Droid
{
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Com.Chteam.Agent.BuglyAgentHelper.Init(this.ApplicationContext, "");

            PlatformConfig.SetWeixin("", "");
            PlatformConfig.SetSinaWeibo("", "", "https://api.weibo.com/oauth2/default.html");

        }
    }
}