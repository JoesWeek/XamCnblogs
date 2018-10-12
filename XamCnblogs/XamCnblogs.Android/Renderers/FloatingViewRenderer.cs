using Android.Content;
using Android.Content.Res;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(FloatingView), typeof(FloatingViewRenderer))]
namespace XamCnblogs.Droid.Renderers {
    public class FloatingViewRenderer : Xamarin.Forms.Platform.Android.ViewRenderer<FloatingView, FloatingActionButton>, IViewPropertyAnimatorListener {
        FloatingActionButton fab;
        public FloatingViewRenderer(Context context) : base(context) {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<FloatingView> e) {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            fab = new FloatingActionButton(Context);
            ViewCompat.SetBackgroundTintList(fab, ColorStateList.ValueOf(Element.ButtonColor.ToAndroid()));
            fab.UseCompatPadding = true;

            var elementImage = Element.Image;
            var imageFile = elementImage?.File;

            if (imageFile != null) {
                fab.SetImageDrawable(Context.Resources.GetDrawable(imageFile));
            }
            fab.Click += delegate (object sender, EventArgs ev) {
                ((IButtonController)Element).SendClicked();
            };
            SetNativeControl(fab);

        }
        protected override void OnLayout(bool changed, int l, int t, int r, int b) {
            base.OnLayout(changed, l, t, r, b);
            Control.BringToFront();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            var fab = Control;
            if (e.PropertyName == nameof(Element.ButtonColor)) {
                ViewCompat.SetBackgroundTintList(fab, ColorStateList.ValueOf(Element.ButtonColor.ToAndroid()));
            }
            if (e.PropertyName == nameof(Element.Image)) {
                var elementImage = Element.Image;
                var imageFile = elementImage?.File;

                if (imageFile != null) {
                    fab.SetImageDrawable(Context.Resources.GetDrawable(imageFile));
                }
            }
            if (e.PropertyName == nameof(Element.ToggleFloatingView)) {
                if (Element.ToggleFloatingView) {
                    if (fab != null) {
                        var animator = ViewCompat.Animate(fab)
                            .SetDuration(500)
                            .ScaleX(0)
                            .ScaleY(0);
                        animator.SetListener(this);
                        animator.Start();
                    }
                }
                else {
                    if (fab != null) {
                        var animator = ViewCompat.Animate(fab)
                        .SetDuration(500)
                        .ScaleX(1)
                        .ScaleY(1);
                        animator.SetListener(this);
                        animator.Start();
                    }
                }
            }
            base.OnElementPropertyChanged(sender, e);

        }
        public void OnAnimationCancel(Android.Views.View view) {
        }

        public void OnAnimationEnd(Android.Views.View view) {
            if (Element.ToggleFloatingView) {
                fab.Visibility = ViewStates.Gone;
            }
            else {
                fab.Visibility = ViewStates.Visible;
            }
        }

        public void OnAnimationStart(Android.Views.View view) {
        }
    }
}