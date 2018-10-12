using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Java.Interop;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(HomeTabbedPage), typeof(HomeTabbedPageRenderer))]
namespace XamCnblogs.Droid.Renderers {
    public class HomeTabbedPageRenderer : TabbedPageRenderer {
        BottomNavigationView bottomNarView;
        public HomeTabbedPageRenderer(Context context) : base(context) {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e) {
            base.OnElementChanged(e);
            var activity = (FormsAppCompatActivity)Context;

            if (e.NewElement != null) {
                var relativeLayout = this.GetChildAt(0) as Android.Widget.RelativeLayout;
                if (relativeLayout != null) {
                    bottomNarView = relativeLayout.GetChildAt(1).JavaCast<BottomNavigationView>();
                    bottomNarView.SetBackgroundColor(Android.Graphics.Color.White);
                    BottomNavigationViewUtils.SetShiftMode(bottomNarView, false, false);
                }
            }
        }
    }
    public static class BottomNavigationViewUtils {
        public static void SetShiftMode(this BottomNavigationView bottomNavigationView, bool enableShiftMode, bool enableItemShiftMode) {
            try {
                var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
                if (menuView == null) {
                    return;
                }


                var shiftMode = menuView.Class.GetDeclaredField("mShiftingMode");

                shiftMode.Accessible = true;
                shiftMode.SetBoolean(menuView, enableShiftMode);
                shiftMode.Accessible = false;
                shiftMode.Dispose();


                for (int i = 0; i < menuView.ChildCount; i++) {
                    var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                    if (item == null)
                        continue;

                    item.SetShiftingMode(enableItemShiftMode);
                    item.SetChecked(item.ItemData.IsChecked);

                }

                menuView.UpdateMenuView();
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Unable to set shift mode: {ex}");
            }
        }
    }

}