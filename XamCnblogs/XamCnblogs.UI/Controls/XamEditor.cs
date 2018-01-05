using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class XamEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(propertyName: nameof(Placeholder),
                returnType: typeof(string),
                declaringType: typeof(CommentEditor),
                defaultValue: default(string));
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
    }
}
