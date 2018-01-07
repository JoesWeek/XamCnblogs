using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Droid.Renderers;

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
                this.Control.SetText(HtmlUtils.GetHtml(Element.Text), Android.Widget.TextView.BufferType.Editable);

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
    }
}