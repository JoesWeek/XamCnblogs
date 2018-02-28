using Android.Widget;
using Plugin.CurrentActivity;
using System;
using Xamarin.Forms;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Logger))]
namespace XamCnblogs.Droid.Helpers
{
    public class Logger : ILog
    {
        public void SaveLog(string tag, Exception ex)
        {
            //构建字符串
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("Tag：\n");
            sb.Append(tag).Append("\n\n");
            sb.Append("异常信息：\n");
            sb.Append(ex.Message).Append("\n\n");
            sb.Append(ex.StackTrace).Append("\n\n");
            sb.Append(ex.Source).Append("\n\n");
            sb.Append(ex.TargetSite).Append("\n\n");
            sb.Append(ex.InnerException).Append("\n\n");
            if (ex.Data != null)
            {
                sb.Append("其他信息：\n");
                foreach (var item in ex.Data)
                {
                    sb.Append(item).Append("\n\n");
                }
            }

            Com.Chteam.Agent.BuglyAgentHelper.PostCatchedException(new Java.Lang.Throwable(sb.ToString()));
        }
    }
}