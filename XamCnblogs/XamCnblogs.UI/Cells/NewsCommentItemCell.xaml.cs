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
	public partial class NewsCommentItemCell : ContentView
	{
		public NewsCommentItemCell ()
		{
			InitializeComponent ();
        }
        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(BookmarksItemCellView), default(ICommand));

        public ICommand DeleteCommand
        {
            get { return GetValue(DeleteCommandProperty) as Command; }
            set { SetValue(DeleteCommandProperty, value); }
        }
        public static readonly BindableProperty EditCommandProperty =
            BindableProperty.Create(nameof(EditCommand), typeof(ICommand), typeof(BookmarksItemCellView), default(ICommand));

        public ICommand EditCommand
        {
            get { return GetValue(EditCommandProperty) as Command; }
            set { SetValue(EditCommandProperty, value); }
        }
    }
}