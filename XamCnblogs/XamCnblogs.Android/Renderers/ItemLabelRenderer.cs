using Android.Content;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.Portable.Interfaces;

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
                this.UpdateNativeControl();

                var itemLabel = (XamCnblogs.UI.Controls.ItemLabel)Element;
                var lineSpacing = itemLabel.LineSpacing;
                var maxLines = itemLabel.MaxLines;

                this.Control.SetLineSpacing(1f, (float)lineSpacing);
                if (maxLines > 1)
                {
                    this.Control.SetMaxLines(maxLines);
                    this.Control.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Text")
            {
                this.UpdateNativeControl();
            }
        }
        private void UpdateNativeControl()
        {
            try
            {
                this.Control.SetText(HtmlUtils.GetHtml(Element.Text), Android.Widget.TextView.BufferType.Normal);
            }
            catch (System.Exception ex)
            {
                this.Control.Text = Element.Text;
                DependencyService.Get<ILog>().SaveLog("ItemLabelRenderer", ex);
            }
        }
    }
}