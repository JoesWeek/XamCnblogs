
using Xamarin.Forms;
using XamCnblogs.UI.Controls;

namespace XamCnblogs.UI.Pages.New
{
    public partial class NewsDetailsHeader : ContentView
	{
		public NewsDetailsHeader ()
		{
			InitializeComponent ();
            this.detailsView.Completed += (e) =>
            {
                (this.Parent as LoadMoreListView).LoadMoreCommand.Execute(null);
            };
        }
	}
}