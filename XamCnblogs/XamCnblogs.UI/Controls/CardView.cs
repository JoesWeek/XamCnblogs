using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class CardView : Frame
    {
        public CardView()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                HasShadow = false;
                BorderColor = (Color)Application.Current.Resources["Divider"];
                CornerRadius = 0;
            }
            else
            {
                CornerRadius = 0;
            }
        }
    }
}
