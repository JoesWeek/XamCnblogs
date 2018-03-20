
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.UI.Controls;

namespace XamCnblogs.UI.Pages.Question
{
    public partial class QuestionsDetailsHeader : ContentView
    {
        public QuestionsDetailsHeader()
        {
            InitializeComponent();

            this.detailsView.Completed += (e) =>
            {
                (this.Parent as LoadMoreListView).LoadMoreCommand.Execute(null);
            };
        }
    }
}