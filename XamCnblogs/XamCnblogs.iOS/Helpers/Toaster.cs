using GlobalToast;
using Xamarin.Forms;
using XamCnblogs.iOS.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Toaster))]
namespace XamCnblogs.iOS.Helpers
{
    public class Toaster : IToast
    {
        public void SendToast(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.ShowToast(message);
            });
        }
    }
}