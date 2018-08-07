using Android.Content;
using Android.Util;

namespace XamCnblogs.Droid.Helpers
{
    public class Utils
    {
        public static int DpToPixel(Context context, float dp)
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