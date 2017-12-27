using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.ViewModel;

namespace XamCnblogs.UI.Pages.About
{
	public partial class AboutPage : ContentPage
    {
        AboutViewModel vm;
        public AboutPage ()
		{
			InitializeComponent ();
            BindingContext = vm = new AboutViewModel();
        }
	}
}