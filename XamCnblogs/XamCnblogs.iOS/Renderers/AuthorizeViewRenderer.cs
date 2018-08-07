using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xam.Plugin.WebView.iOS;
using Xamarin.Forms;
using XamCnblogs.iOS.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(AuthorizeView), typeof(AuthorizeViewRenderer))]
namespace XamCnblogs.iOS.Renderers
{
    public class AuthorizeViewRenderer : FormsWebViewRenderer
    {
        public AuthorizeViewRenderer() : base()
        {
            var cookies = NSHttpCookieStorage.SharedStorage;

            foreach (var cookie in cookies.Cookies)
            {
                cookies.DeleteCookie(cookie);
            }
        }
    }
}