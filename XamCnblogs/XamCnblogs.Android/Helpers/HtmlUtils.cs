using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using Java.IO;
using Java.Lang;
using Java.Net;
using Org.Xml.Sax;
using System;
using System.IO;
using System.Threading.Tasks;

namespace XamCnblogs.Droid.Helpers
{
    public class HtmlUtils
    {
        public static ISpanned GetHtml(string html, FromHtmlOptions flags = FromHtmlOptions.ModeLegacy)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                return Html.FromHtml(html, flags);
            }
            else
            {
                return Html.FromHtml(html);
            }
        }
    }
}