using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Widget;
using Xamarin.Forms;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;
using static Android.Widget.AbsListView;
using AListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(LoadMoreListView), typeof(LoadMoreListViewRenderer))]
namespace XamCnblogs.Droid.Renderers
{
    public class LoadMoreListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer, Android.Widget.AbsListView.IOnScrollListener
    {
        private bool scrollFlag = false;// 标记是否滑动
        private int lastVisibleItemPosition = 0;// 标记上次滑动位置
        private HomeTabbedPage HomePage;
        public LoadMoreListViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var aListView = (AListView)Control;
                aListView.VerticalScrollBarEnabled = false;
                aListView.SetOnScrollListener(this);
                var _refresh = (SwipeRefreshLayout)aListView.Parent;
                if (_refresh != null)
                {
                    _refresh.SetColorSchemeResources(Resource.Color.primary);
                }
                HomePage = this.Element.Parent.Parent.Parent as HomeTabbedPage;
            }
        }

        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount)
        {
            if (scrollFlag)
            {
                if (firstVisibleItem > lastVisibleItemPosition)
                {
                    // 上滑
                    if (HomePage != null)
                    {
                        HomePage.ToggleFloatingActionButton = true;
                    }
                }
                else if (firstVisibleItem < lastVisibleItemPosition)
                {
                    // 下滑
                    if (HomePage != null)
                    {
                        HomePage.ToggleFloatingActionButton = false;
                    }
                }
                else
                {
                    return;
                }
                lastVisibleItemPosition = firstVisibleItem;
            }
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState)
        {
            switch (scrollState)
            {
                // 当不滚动时
                case ScrollState.Idle:// 是当屏幕停止滚动时
                    scrollFlag = false;
                    break;
                case ScrollState.TouchScroll:// 滚动时
                    scrollFlag = true;
                    break;
                case ScrollState.Fling:// 是当用户由于之前划动屏幕并抬起手指，屏幕产生惯性滑动时
                    scrollFlag = false;
                    break;
            }
        }
    }
}