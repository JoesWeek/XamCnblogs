
using Android.App;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Shares))]

namespace XamCnblogs.Droid.Helpers
{
    public class Shares : IShares
    {
        static Activity activity;
        public static void Init(Activity a)
        {
            activity = a;
        }
        void IShares.Shares(string url, string title)
        {
            var sharesWidget = new SharesWidget(activity);
            sharesWidget.Open(url, title);
        }
        void IShares.SharesIcon(string url, string title, object icon)
        {
            var sharesWidget = new SharesWidget(activity);
            sharesWidget.Open(url, title, icon);
        }

    }
}