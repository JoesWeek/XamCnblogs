using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class CommentEditor : Editor
    {
        public CommentEditor()
        {
        }
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
