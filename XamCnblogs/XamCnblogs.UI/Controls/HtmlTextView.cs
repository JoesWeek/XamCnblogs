using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class HtmlTextView : View
    {
        public double LineSpacing { get; set; }
        public int MaxLines { get; set; }
        public double FontSize { get; set; }
        public Color TextColor { get; set; }
        public new Style Style { get; set; }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(propertyName: nameof(Text),
                returnType: typeof(string),
                declaringType: typeof(HtmlTextView),
                defaultValue: default(string));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
