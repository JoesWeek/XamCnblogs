using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class DetailsView : View
    {
        public DetailsView()
        {
            HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand;            
        }
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(propertyName: nameof(Source),
                returnType: typeof(string),
                declaringType: typeof(DetailsView),
                defaultValue: default(string));
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public void OnCompleted()
        {
            Completed?.Invoke(this);
        }

        public event DetailsCompletedEventHandler Completed;
    }
    public delegate void DetailsCompletedEventHandler(object sender);
}