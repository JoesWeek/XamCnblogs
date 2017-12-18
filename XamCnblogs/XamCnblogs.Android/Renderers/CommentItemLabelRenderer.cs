using Android.Content;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.CommentItemLabel), typeof(CommentItemLabelRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class CommentItemLabelRenderer : LabelRenderer
    {
        public CommentItemLabelRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                this.Control.SetText(Html.FromHtml(this.Control.Text), TextView.BufferType.Normal);

                var itemLabel = (XamCnblogs.UI.Controls.CommentItemLabel)Element;
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