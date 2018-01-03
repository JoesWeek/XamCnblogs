using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;

[assembly: ExportRenderer(typeof(SearchBar), typeof(XamSearchBarRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class XamSearchBarRenderer : SearchBarRenderer
    {
        public XamSearchBarRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
                return;

            UpdateSearchIcon();
            UpdateCursorColor();
            UpdateSearchPlate();
        }

        void UpdateSearchPlate()
        {
            var searchId = Control.Resources.GetIdentifier("android:id/search_plate", null, null);
            if (searchId == 0)
                return;

            var image = FindViewById<Android.Views.View>(searchId);
            if (image == null)
                return;

            image.SetBackgroundColor(Android.Graphics.Color.Transparent);
        }


        void UpdateSearchIcon()
        {
            try
            {
                var searchId = Control.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                if (searchId == 0)
                    return;


                var image = this.FindViewById<ImageView>(searchId);
                if (image == null)
                    return;

                image.SetImageResource(Resource.Drawable.toolbar_search);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get icon" + ex);
            }
        }

        void UpdateCursorColor()
        {
            AutoCompleteTextView textView = null;
            try
            {
                var searchId = Control.Resources.GetIdentifier("android:id/search_src_text", null, null);
                if (searchId == 0)
                    return;


                textView = this.FindViewById<AutoCompleteTextView>(searchId);
                if (textView == null)
                    return;


                var theClass = Java.Lang.Class.FromType(typeof(TextView));
                var theField = theClass.GetDeclaredField("mCursorDrawableRes");
                theField.Accessible = true;
                theField.Set(textView, Resource.Drawable.search_cursor);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get icon" + ex);
            }

            try
            {
                if (textView == null)
                    return;
                textView.SetBackgroundResource(Resource.Drawable.searchview_background);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to get icon" + ex);
            }

        }
    }
}