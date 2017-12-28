using System;

using Android.App;
using Android.OS;
using Android.Runtime;
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
        }
    }
}