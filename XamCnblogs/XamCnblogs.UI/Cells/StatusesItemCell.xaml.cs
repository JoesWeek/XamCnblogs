using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamCnblogs.UI.Cells
{
	public partial class StatusesItemCell : ContentView
	{
		public StatusesItemCell ()
		{
			InitializeComponent ();
        }
        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(StatusesItemCell), default(ICommand));

        public ICommand DeleteCommand
        {
            get { return GetValue(DeleteCommandProperty) as Command; }
            set { SetValue(DeleteCommandProperty, value); }
        }
    }
}