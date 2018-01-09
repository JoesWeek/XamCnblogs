using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.HtmlTextView), typeof(HtmlTextViewRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class HtmlTextViewRenderer : ViewRenderer<XamCnblogs.UI.Controls.HtmlTextView, TextView>
    {
        private Org.Sufficientlysecure.Htmltextview.HtmlTextView htmlTextView;
        public HtmlTextViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<XamCnblogs.UI.Controls.HtmlTextView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                try
                {
                    if (htmlTextView == null)
                        htmlTextView = new Org.Sufficientlysecure.Htmltextview.HtmlTextView(this.Context);
                    if (this.Element.FontSize > 0)
                        htmlTextView.TextSize = float.Parse(this.Element.FontSize.ToString());
                    if (this.Element.TextColor != new Color())
                        htmlTextView.SetTextColor(this.Element.TextColor.ToAndroid());

                    var textView = (XamCnblogs.UI.Controls.HtmlTextView)Element;
                    var lineSpacing = textView.LineSpacing;
                    var maxLines = textView.MaxLines;

                    htmlTextView.SetLineSpacing(1f, (float)lineSpacing);
                    if (maxLines > 1)
                    {
                        htmlTextView.SetMaxLines(maxLines);
                        htmlTextView.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
                    }
                    SetNativeControl(htmlTextView);
                    this.UpdateNativeControl();
                }
                catch (System.Exception ex)
                {
                    new Logger().SendLog("HtmlTextViewRenderer：" + ex.Message);
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
            if (htmlTextView != null)
                htmlTextView.SetHtml(this.Element.Text, new Org.Sufficientlysecure.Htmltextview.HtmlHttpImageGetter(htmlTextView));
        }
    }
}