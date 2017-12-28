using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Shares))]

namespace XamCnblogs.Droid.Helpers
{
    public class Shares : IShares
    {
        void IShares.Shares(string url, string title)
        {
            var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
            Device.BeginInvokeOnMainThread(() =>
            {
                //var sharesWidget = new SharesWidget(context);
                //sharesWidget.Open(url, title);
            });
        }
    }
}