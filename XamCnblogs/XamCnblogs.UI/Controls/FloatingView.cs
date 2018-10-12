using Xamarin.Forms;

namespace XamCnblogs.UI.Controls {
    public class FloatingView : Button {

        public FloatingView() {
        }
        public static BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(FloatingView), Color.Accent);
        public Color ButtonColor {
            get {
                return (Color)GetValue(ButtonColorProperty);
            }
            set {
                SetValue(ButtonColorProperty, value);
            }
        }
        public static readonly BindableProperty ToggleFloatingViewProperty = BindableProperty.Create(propertyName: nameof(ToggleFloatingView),
                returnType: typeof(bool),
                declaringType: typeof(FloatingView),
                defaultValue: false);
        public bool ToggleFloatingView {
            get { return (bool)GetValue(ToggleFloatingViewProperty); }
            set { SetValue(ToggleFloatingViewProperty, value); }
        }
    }

}
