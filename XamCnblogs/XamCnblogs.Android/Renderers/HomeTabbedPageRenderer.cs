using Android.Content;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using FormsToolkit;
using Java.Interop;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.UI.Controls;
using XamCnblogs.UI.Pages.Question;
using XamCnblogs.UI.Pages.Status;

[assembly: ExportRenderer(typeof(HomeTabbedPage), typeof(HomeTabbedPageRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class HomeTabbedPageRenderer : TabbedPageRenderer
    {
        BottomNavigationView bottomNarView;
        FloatingActionButton floating;
        public HomeTabbedPageRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            var activity = (FormsAppCompatActivity)Context;

            if (e.NewElement != null)
            {
                var relativeLayout = this.GetChildAt(0) as Android.Widget.RelativeLayout;
                if (relativeLayout != null)
                {
                    bottomNarView = relativeLayout.GetChildAt(1).JavaCast<BottomNavigationView>();
                    BottomNavigationViewUtils.SetShiftMode(bottomNarView, false, false);

                    floating = activity.LayoutInflater.Inflate(Resource.Layout.FloatingActionButton, null).JavaCast<FloatingActionButton>();

                    floating.Click += async delegate (object sender, EventArgs ev)
                    {
                        if (UserTokenSettings.Current.HasExpiresIn())
                        {
                            MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
                        }
                        else
                        {
                            if (this.Element.CurrentPage is StatusesTopTabbedPage)
                            {
                                await NavigationService.PushAsync(this.Element.Navigation, new StatusesEditPage(new Statuses()));
                            }
                            else if (this.Element.CurrentPage is QuestionsTopTabbedPage)
                            {
                                await NavigationService.PushAsync(this.Element.Navigation, new QuestionsEditPage(new Questions()));
                            }
                        }
                    };

                    var layoutParams = new Android.Widget.RelativeLayout.LayoutParams(Android.Widget.RelativeLayout.LayoutParams.WrapContent, Android.Widget.RelativeLayout.LayoutParams.WrapContent);
                    layoutParams.AddRule(LayoutRules.AlignParentRight);
                    layoutParams.AddRule(LayoutRules.AlignParentBottom);
                    layoutParams.AddRule(LayoutRules.Above, bottomNarView.Id);

                    floating.LayoutParameters = layoutParams;

                    relativeLayout.AddView(floating);
                }
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var page = this.Element as HomeTabbedPage;
            if (e.PropertyName == nameof(page.HasFloatingActionButton))
            {
                if (page.HasFloatingActionButton)
                {
                    floating.Visibility = ViewStates.Visible;
                }
                else
                {
                    floating.Visibility = ViewStates.Gone;
                }
            }
            else if (e.PropertyName == nameof(page.CurrentPage))
            {
                if (page.CurrentPage is StatusesTopTabbedPage)
                {
                    page.HasFloatingActionButton = true;
                    page.ToggleFloatingActionButton = false;
                }
                else if (page.CurrentPage is QuestionsTopTabbedPage)
                {
                    page.HasFloatingActionButton = true;
                    page.ToggleFloatingActionButton = false;
                }
                else
                {
                    page.HasFloatingActionButton = false;
                }
            }
            else if (e.PropertyName == nameof(page.ToggleFloatingActionButton))
            {
                if (page.ToggleFloatingActionButton)
                {
                    if (floating != null)
                    {
                        var animator = ViewCompat.Animate(floating)
                            .SetDuration(500)
                            .ScaleX(0)
                            .ScaleY(0);
                        animator.Start();
                    }
                }
                else
                {
                    if (floating != null)
                    {
                        var animator = ViewCompat.Animate(floating)
                        .SetDuration(500)
                        .ScaleX(1)
                        .ScaleY(1);
                        animator.Start();
                    }
                }
            }
        }
        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            Context context = Context;
            var layoutParams = floating.LayoutParameters as Android.Widget.RelativeLayout.LayoutParams;
            layoutParams.BottomMargin = bottomNarView.Height + Utils.DpToPixel(this.Context, 16);
            layoutParams.RightMargin = Utils.DpToPixel(this.Context, 16);

            base.OnLayout(changed, left, top, right, bottom);
        }
    }
    public static class BottomNavigationViewUtils
    {
        public static void SetShiftMode(this BottomNavigationView bottomNavigationView, bool enableShiftMode, bool enableItemShiftMode)
        {
            try
            {
                var menuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;
                if (menuView == null)
                {
                    return;
                }


                var shiftMode = menuView.Class.GetDeclaredField("mShiftingMode");

                shiftMode.Accessible = true;
                shiftMode.SetBoolean(menuView, enableShiftMode);
                shiftMode.Accessible = false;
                shiftMode.Dispose();


                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = menuView.GetChildAt(i) as BottomNavigationItemView;
                    if (item == null)
                        continue;

                    item.SetShiftingMode(enableItemShiftMode);
                    item.SetChecked(item.ItemData.IsChecked);

                }

                menuView.UpdateMenuView();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unable to set shift mode: {ex}");
            }
        }
    }

}