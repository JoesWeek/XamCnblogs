using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Java.Lang;
using Microsoft.AppCenter.Crashes;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.ItemLabel), typeof(ItemLabelRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class ItemLabelRenderer : LabelRenderer
    {
        public ItemLabelRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var itemLabel = (XamCnblogs.UI.Controls.ItemLabel)Element;
                var lineSpacing = itemLabel.LineSpacing;
                var maxLines = itemLabel.MaxLines;

                this.Control.SetLineSpacing(1f, (float)lineSpacing);
                if (maxLines > 1)
                {
                    this.Control.SetMaxLines(maxLines);
                    this.Control.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
                }
                this.UpdateNativeControl();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Text" || e.PropertyName == "IsLucky")
            {
                this.UpdateNativeControl();
            }
        }
        private void UpdateNativeControl()
        {
            try
            {
                if ((Element as ItemLabel).IsLucky)
                {
                    ImageSpan span = new ImageSpan(this.Context, Resource.Drawable.luckyStar);
                    SpannableString spanStr = new SpannableString(Element.Text);
                    spanStr.SetSpan(span, spanStr.Length() - 1, spanStr.Length(), SpanTypes.InclusiveExclusive);
                    this.Control.SetText(spanStr, Android.Widget.TextView.BufferType.Normal);
                }
                else
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
                    {
                        this.Control.SetText(Html.FromHtml(Element.Text, FromHtmlOptions.ModeLegacy), Android.Widget.TextView.BufferType.Normal);
                    }
                    else
                    {
                        this.Control.SetText(Html.FromHtml(Element.Text), Android.Widget.TextView.BufferType.Normal);
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.Control.Text = Element.Text;
                Crashes.TrackError(ex);
            }
        }
    }
}