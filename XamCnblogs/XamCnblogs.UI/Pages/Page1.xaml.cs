using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamCnblogs.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page1 : ContentPage
	{
		public Page1 ()
		{
			InitializeComponent ();
            this.HtmlTextView.Text = "图片测试<img src='https://www.cnblogs.com/images/logo_small.gif' alt='博客园Logo'>";

        }
	}
}