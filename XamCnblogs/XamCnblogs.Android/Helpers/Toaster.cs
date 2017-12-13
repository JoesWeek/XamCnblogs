using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Toaster))]
namespace XamCnblogs.Droid.Helpers
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(context, message, ToastLength.Long).Show();
            });
        }
    }
}