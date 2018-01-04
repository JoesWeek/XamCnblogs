using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using FFImageLoading;
using Org.Xml.Sax;
using System;
using System.Threading.Tasks;

namespace XamCnblogs.Droid.Helpers
{
    public class HtmlUtils
    {
        public static ISpanned GetHtml(string html, FromHtmlOptions flags = FromHtmlOptions.ModeLegacy)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                return Html.FromHtml(html, flags, new XamImageGetter(), new XamTagHandler());
            }
            else
            {
                return Html.FromHtml(html, new XamImageGetter(), new XamTagHandler());
            }
        }
        public class XamImageGetter : Java.Lang.Object, Html.IImageGetter
        {
            public Drawable GetDrawable(string source)
            {
                Drawable drawable = null;
                try
                {
                    Task.Run(async () =>
                    {
                        drawable = Drawable.CreateFromStream(await ImageService.Instance.LoadUrl(source).Stream(System.Threading.CancellationToken.None), null);
                        drawable.SetBounds(0, 0, drawable.IntrinsicWidth, drawable.IntrinsicHeight);
                    });
                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block  
                }
                return drawable;
            }
        }
        public class XamTagHandler : Java.Lang.Object, Html.ITagHandler
        {
            public void HandleTag(bool opening, string tag, IEditable output, IXMLReader xmlReader)
            {
            }
        }
    }
}