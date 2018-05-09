using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using Xamarin.Forms;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.UI.Cells
{
    public partial class ArticlesItemCell : ViewCell
    {
        public ArticlesItemCell()
        {
            InitializeComponent();
            ffimageloading.Transformations.Add(new CircleTransformation());
        }
        protected override void OnBindingContextChanged()
        {
            this.ffimageloading.Source = null;
            var item = BindingContext as Articles;

            if (item == null)
                return;

            this.ffimageloading.Source = item.Avatar;

            base.OnBindingContextChanged();
        }
    }
}