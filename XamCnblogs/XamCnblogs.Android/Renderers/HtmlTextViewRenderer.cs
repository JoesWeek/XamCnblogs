using Android.App;
using Android.Content;
using Android.Views;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(XamCnblogs.UI.Controls.HtmlTextView), typeof(HtmlTextViewRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class HtmlTextViewRenderer : ViewRenderer<XamCnblogs.UI.Controls.HtmlTextView, Android.Views.View>
    {
        public HtmlTextViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<XamCnblogs.UI.Controls.HtmlTextView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var customView = new Org.Sufficientlysecure.Htmltextview.HtmlTextView(this.Context);

                if (this.Element.Style != null)
                {
                    var setters = this.Element.Style.Setters;
                    var fontSize = setters.Where(s => s.Property.PropertyName == "FontSize").FirstOrDefault();
                    if (fontSize != null)
                    {
                        customView.TextSize = float.Parse(fontSize.Value.ToString());
                    }
                    var textColor = setters.Where(s => s.Property.PropertyName == "TextColor").FirstOrDefault();
                    if (textColor != null)
                    {
                        customView.SetTextColor(((Color)textColor.Value).ToAndroid());
                    }
                }

                if (this.Element.FontSize > 0)
                    customView.TextSize = float.Parse(this.Element.FontSize.ToString());
                if (this.Element.TextColor != new Color())
                    customView.SetTextColor(this.Element.TextColor.ToAndroid());
                customView.SetHtml(this.Element.Text, new Org.Sufficientlysecure.Htmltextview.HtmlHttpImageGetter(customView));
                var contentView = (ViewGroup)customView.Parent;
                if (contentView != null)
                {
                    var oldLayout = contentView.GetChildAt(1);
                    contentView.RemoveView(oldLayout);
                }
                SetNativeControl(customView);
            }
        }
    }
}