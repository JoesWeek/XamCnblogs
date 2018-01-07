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
                return Html.FromHtml(html, flags, new HtmlHttpImageGetter(), new XamTagHandler());
            }
            else
            {
                return Html.FromHtml(html, new HtmlHttpImageGetter(), new XamTagHandler());
            }
        }
        public class HtmlHttpImageGetter : Java.Lang.Object, Html.IImageGetter
        {
            TextView container;
            URI baseUri;
            bool matchParentWidth;

            private bool compressImage = false;
            private int qualityImage = 50;

            public HtmlHttpImageGetter()
            {
            }

            public HtmlHttpImageGetter(TextView textView)
            {
                this.container = textView;
                this.matchParentWidth = false;
            }

            public HtmlHttpImageGetter(TextView textView, string baseUrl)
            {
                this.container = textView;
                if (baseUrl != null)
                {
                    this.baseUri = URI.Create(baseUrl);
                }
            }

            public HtmlHttpImageGetter(TextView textView, string baseUrl, bool matchParentWidth)
            {
                this.container = textView;
                this.matchParentWidth = matchParentWidth;
                if (baseUrl != null)
                {
                    this.baseUri = URI.Create(baseUrl);
                }
            }

            public void enableCompressImage(bool enable)
            {
                enableCompressImage(enable, 50);
            }

            public void enableCompressImage(bool enable, int quality)
            {
                compressImage = enable;
                qualityImage = quality;
            }

            public Drawable GetDrawable(string source)
            {
                //UrlDrawable urlDrawable = new UrlDrawable();

                //// get the actual source
                //ImageGetterAsyncTask asyncTask = new ImageGetterAsyncTask(urlDrawable, this, container,
                //        matchParentWidth, compressImage, qualityImage);

                //asyncTask.execute(source);

                //// return reference to URLDrawable which will asynchronously load the image specified in the src tag
                //return urlDrawable;
                return null;
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