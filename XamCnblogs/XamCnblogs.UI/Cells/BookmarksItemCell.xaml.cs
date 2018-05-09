using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.UI.Pages.Account;

namespace XamCnblogs.UI.Cells
{
    public partial class BookmarksItemCellView : ViewCell
    {
        public BookmarksItemCellView()
        {
            InitializeComponent();
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