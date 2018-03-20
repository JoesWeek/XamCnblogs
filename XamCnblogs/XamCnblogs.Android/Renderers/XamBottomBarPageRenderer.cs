using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
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
                //var frameLayout = _bottomBar.FindViewById<FrameLayout>(Resource.Id.bb_user_content_container);

                //var view = new TextView(this.Context);
                //view.Text = "test";
                //view.LayoutParameters = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent);
                //frameLayout.AddView(view,0);
                //var items = frameLayout.GetChildAt(1);
                //items.LayoutParameters = new Android.Widget.FrameLayout.LayoutParams(Android.Widget.FrameLayout.LayoutParams.MatchParent, Android.Widget.FrameLayout.LayoutParams.MatchParent) { TopMargin = DpToPixel(this.Context, 56) };
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
        public  int DpToPixel(Context context, float dp)
        {
            var resources = context.Resources;
            var metrics = resources.DisplayMetrics;

            try
            {
                return (int)(dp * ((int)metrics.DensityDpi / 160f));
            }
            catch (Java.Lang.NoSuchFieldError)
            {
                return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, metrics);
            }

        }
    }
}