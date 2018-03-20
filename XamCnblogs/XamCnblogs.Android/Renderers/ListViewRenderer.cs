using Android.Content;
using Android.Support.V4.Widget;
using Xamarin.Forms;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;
using AListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(LoadMoreListView), typeof(LoadMoreListViewRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class LoadMoreListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer
    {
        public LoadMoreListViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var aListView = (AListView)Control;
                aListView.VerticalScrollBarEnabled = false;
                var _refresh = (SwipeRefreshLayout)aListView.Parent;
                if (_refresh != null)
                {
                    _refresh.SetColorSchemeResources(Resource.Color.primary);
                }
            }
        }
    }
}