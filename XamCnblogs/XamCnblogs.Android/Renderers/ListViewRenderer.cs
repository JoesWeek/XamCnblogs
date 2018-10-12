using Android.Content;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Widget;
using System.ComponentModel;
using Xamarin.Forms;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(LoadMoreListView), typeof(LoadMoreListViewRenderer))]
namespace XamCnblogs.Droid.Renderers {
    public class LoadMoreListViewRenderer : Xamarin.Forms.Platform.Android.ListViewRenderer, Android.Widget.AbsListView.IOnScrollListener {
        private bool scrollFlag = false;// 标记是否滑动
        private int lastVisibleItemPosition = 0;// 标记上次滑动位置
        private FloatingView floatingView;
        public LoadMoreListViewRenderer(Context context) : base(context) {

        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.ListView> e) {
            base.OnElementChanged(e);

            if (e.NewElement != null) {
                Control.VerticalScrollBarEnabled = false;
                var _refresh = (SwipeRefreshLayout)Control.Parent;
                if (_refresh != null) {
                    _refresh.SetColorSchemeResources(Resource.Color.primary);
                }
                if ((this.Element as LoadMoreListView).HasFloatingView) {
                    Control.SetOnScrollListener(this);
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
            base.OnElementPropertyChanged(sender, e);
            var page = this.Element as LoadMoreListView;
            if (e.PropertyName == nameof(page.HasFloatingView)) {
                if (page.HasFloatingView) {
                    Control.SetOnScrollListener(this);
                }
                else {
                    Control.SetOnScrollListener(null);
                }
            }
        }
        public void OnScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
            if (scrollFlag) {
                if (firstVisibleItem > lastVisibleItemPosition) {
                    // 上滑
                    (this.Element as LoadMoreListView).OnFloatingChanged(true);
                }
                else if (firstVisibleItem < lastVisibleItemPosition) {
                    // 下滑
                    (this.Element as LoadMoreListView).OnFloatingChanged(false);
                }
            }
            else {
                return;
            }
            lastVisibleItemPosition = firstVisibleItem;
        }

        public void OnScrollStateChanged(AbsListView view, [GeneratedEnum] ScrollState scrollState) {
            switch (scrollState) {
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