using Android.Widget;
using Microsoft.AppCenter.Crashes;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(VersionNameer))]
namespace XamCnblogs.Droid.Helpers
{
    public class VersionNameer : IVersionName
    {
        public string GetVersionName()
        {
            try
            {

                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
                return packageInfo.VersionName;
            }
            catch (System.Exception ex)
            {
                Crashes.TrackError(ex);
            }
            return "";
        }
    }
}