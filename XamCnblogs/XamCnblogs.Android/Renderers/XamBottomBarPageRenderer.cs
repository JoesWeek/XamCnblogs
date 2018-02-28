using BottomBar.XamarinForms;
using BottomNavigationBar.Listeners;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(XamBottomBarPage), typeof(XamCnblogs.Droid.Renderers.XamBottomBarPageRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class XamBottomBarPageRenderer : BottomBar.Droid.Renderers.BottomBarPageRenderer, IOnTabClickListener
    {
        BottomNavigationBar.BottomBar _bottomBar;
        public XamBottomBarPageRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BottomBarPage> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _bottomBar = (BottomNavigationBar.BottomBar)this.GetChildAt(0);
                _bottomBar.SetOnTabClickListener(this);

            }
        }
        public new void OnTabSelected(int position)
        {
            var bottomBarPage = Element as XamBottomBarPage;
            bottomBarPage.CurrentPage = Element.Children[position];
            Element.Title = Element.Children[position].Title;
        }

        public new void OnTabReSelected(int position)
        {
        }
    }
}