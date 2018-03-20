
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.UI.Controls;

namespace XamCnblogs.UI.Pages.Article
{
    public partial class ArticlesDetailsHeader : ContentView
    {
        public ArticlesDetailsHeader()
        {
            InitializeComponent();

            this.detailsView.Completed += (e) =>
            {
                (this.Parent as LoadMoreListView).LoadMoreCommand.Execute(null);
            };
        }
    }
}