using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.ItemLabel), typeof(ItemLabelRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class ItemLabelRenderer : LabelRenderer
    {
        public ItemLabelRenderer(Context context) : base(context)
        {

        }
        protected XamCnblogs.UI.Controls.ItemLabel ItemLabel { get; private set; }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                this.ItemLabel = (XamCnblogs.UI.Controls.ItemLabel)Element;
            }

            var lineSpacing = this.ItemLabel.LineSpacing;
            var maxLines = this.ItemLabel.MaxLines;

            this.Control.SetLineSpacing(1f, (float)lineSpacing);
            if (maxLines > 1)
            {
                this.Control.SetMaxLines(maxLines);
                this.Control.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
            }
            this.UpdateLayout();
        }
    }
}