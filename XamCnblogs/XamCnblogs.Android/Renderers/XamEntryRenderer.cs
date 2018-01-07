using Android.Content;
using Android.Runtime;
using Android.Widget;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(XamEntry), typeof(XamEntryRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class XamEntryRenderer : EntryRenderer
    {
        public XamEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.Control.Background = null;

                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.color_cursor);
            }
        }
    }
}