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
            Com.Chteam.Agent.BuglyAgentHelper.Init(this.ApplicationContext, "e2fab3d122");

            PlatformConfig.SetWeixin("wxcf8f642a8aa4c630", "76ebc29b4194164aee32eedff2e17900");
            PlatformConfig.SetSinaWeibo("1422675167", "02975c36afd93d3ae983f8da9e596b86", "https://api.weibo.com/oauth2/default.html");

        }
    }
}