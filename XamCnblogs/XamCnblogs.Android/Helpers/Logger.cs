using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Logger))]
namespace XamCnblogs.Droid.Helpers
{
    public class Logger : ILog
    {
        public void SendLog(string message)
        {
            Com.Chteam.Agent.BuglyAgentHelper.PostCatchedException(new Java.Lang.Throwable(message));
        }
    }
}