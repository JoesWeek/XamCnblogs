using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.UI.Cells
{
	public partial class NewsItemCell : ViewCell
    {
		public NewsItemCell ()
		{
			InitializeComponent ();
        }
        protected override void OnBindingContextChanged()
        {
            this.ffimageloading.Source = null;
            var item = BindingContext as News;

            if (item == null)
                return;

            this.ffimageloading.Source = item.TopicIcon;

            base.OnBindingContextChanged();
        }
    }
}