using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.UI.Controls;

namespace XamCnblogs.UI.Pages.Question
{
	public partial class AnswersDetailsHeader : ContentView
	{
		public AnswersDetailsHeader ()
		{
			InitializeComponent ();
            this.detailsView.Completed += (e) =>
            {
                (this.Parent as LoadMoreListView).LoadMoreCommand.Execute(null);
            };
        }
	}
}