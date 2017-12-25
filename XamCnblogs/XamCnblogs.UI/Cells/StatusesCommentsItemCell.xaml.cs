using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.UI.Cells
{
    public partial class StatusesCommentsItemCell : ContentView
	{
		public StatusesCommentsItemCell ()
		{
			InitializeComponent ();
        }
        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(StatusesCommentsItemCell), default(ICommand));

        public ICommand DeleteCommand
        {
            get { return GetValue(DeleteCommandProperty) as Command; }
            set { SetValue(DeleteCommandProperty, value); }
        }
    }
}