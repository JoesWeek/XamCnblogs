using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class HomeTabbedPage : TabbedPage
    {
        public HomeTabbedPage()
        {

        }
        public static readonly BindableProperty HasFloatingActionButtonProperty = BindableProperty.Create(propertyName: nameof(HasFloatingActionButton),
                returnType: typeof(bool),
                declaringType: typeof(HomeTabbedPage),
                defaultValue: false);
        public bool HasFloatingActionButton
        {
            get { return (bool)GetValue(HasFloatingActionButtonProperty); }
            set { SetValue(HasFloatingActionButtonProperty, value); }
        }
        public static readonly BindableProperty ToggleFloatingActionButtonProperty = BindableProperty.Create(propertyName: nameof(ToggleFloatingActionButton),
                returnType: typeof(bool),
                declaringType: typeof(HomeTabbedPage),
                defaultValue: false);
        public bool ToggleFloatingActionButton
        {
            get { return (bool)GetValue(ToggleFloatingActionButtonProperty); }
            set { SetValue(ToggleFloatingActionButtonProperty, value); }
        }
    }
}
