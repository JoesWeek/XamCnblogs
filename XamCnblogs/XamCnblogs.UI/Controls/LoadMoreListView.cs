using XamCnblogs.Portable.Helpers;
using FormsToolkit;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class LoadMoreListView : ListView
    {
        //正在加载
        private StackLayout loadingPage;
        //错误
        private StackLayout failPage;
        //加载更多错误
        private StackLayout errorPage;
        //结束
        private StackLayout endPage;
        //没有数据
        private StackLayout nodataPage;
        //没有登录
        private StackLayout nologinPage;
        public LoadMoreListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            loadingPage = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HeightRequest = 50,
                Children = {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Orientation=StackOrientation.Horizontal,
                        Children ={
                                new ActivityIndicator{
                                    IsRunning=true,
                                    WidthRequest=20,
                                    HeightRequest=20,
                                    Color=(Color)Application.Current.Resources["Primary"],
                                    VerticalOptions = LayoutOptions.CenterAndExpand
                                },
                                new Label
                                {
                                    Text = "正在加载中",
                                    Style=Application.Current.Resources["SecondaryTextStyle"] as Style,
                                    VerticalOptions = LayoutOptions.CenterAndExpand
                                }
                         }
                     }
                }
            };
            endPage = new StackLayout
            {
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                      new Label
                      {
                           Text = "没有更多了",
                           Style=Application.Current.Resources["SecondaryTextStyle"] as Style,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                       }
                 }
            };
            failPage = new StackLayout
            {
                HeightRequest = FailHeight,
                Children = {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Children = {
                            new Label
                            {
                                Text = "似乎出了点问题",
                                Style = Application.Current.Resources["SecondaryTextStyle"] as Style      ,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                            },
                            new Label
                            {
                                Text = "重新加载",
                                Style = Application.Current.Resources["SecondaryTextStyle"] as Style,
                                TextColor=(Color)Application.Current.Resources["Primary"],
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                            }
                        }
                    }
                }
            };
            var failGestureRecognizer = new TapGestureRecognizer();
            failGestureRecognizer.Tapped += (s, e) =>
            {
                if (LoadStatus == LoadMoreStatus.StausFail)
                    RefreshCommand.Execute(null);
            };
            failPage.GestureRecognizers.Add(failGestureRecognizer);

            nodataPage = new StackLayout
            {
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                      new Label
                      {
                           Text = "还没有内容",
                           Style=Application.Current.Resources["SecondaryTextStyle"] as Style,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                       }
                 }
            };

            errorPage = new StackLayout
            {
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                      new Label
                      {
                           Text = "加载失败，点击重试",
                           Style=Application.Current.Resources["SecondaryTextStyle"] as Style,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                       }
                 }
            };
            var errorGestureRecognizer = new TapGestureRecognizer();
            errorGestureRecognizer.Tapped += (s, e) =>
            {
                if (LoadStatus == LoadMoreStatus.StausError && LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            };
            errorPage.GestureRecognizers.Add(errorGestureRecognizer);

            nologinPage = new StackLayout
            {
                HeightRequest = FailHeight,
                Children = {
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        Children = {
                            new Label
                            {
                                Text = "还未登录",
                                Style = Application.Current.Resources["SecondaryTextStyle"] as Style      ,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                            },
                            new Label
                            {
                                Text = "马上登录",
                                Style = Application.Current.Resources["SecondaryTextStyle"] as Style,
                                TextColor=(Color)Application.Current.Resources["Primary"],
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                            }
                        }
                    }
                }
            };
            var nologinGestureRecognizer = new TapGestureRecognizer();
            nologinGestureRecognizer.Tapped += (s, e) =>
            {
                if (LoadStatus == LoadMoreStatus.StausNologin)
                    MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            };
            nologinPage.GestureRecognizers.Add(nologinGestureRecognizer);


            ItemAppearing += LoadMoreListView_ItemAppearing;

        }
        void LoadMoreListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
            {
                if (CanLoadMore && LoadStatus == LoadMoreStatus.StausDefault)
                {
                    if (CanLoadMore && (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null)))
                        LoadMoreCommand.Execute(null);
                }
            }
        }

        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(LoadMoreListView), default(ICommand));

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }
        public static readonly BindableProperty CanLoadMoreProperty = BindableProperty.Create(nameof(CanLoadMore), typeof(bool), typeof(LoadMoreListView), false);
        public bool CanLoadMore
        {
            get { return (bool)GetValue(CanLoadMoreProperty); }
            set { SetValue(CanLoadMoreProperty, value); }
        }

        public static readonly BindableProperty LoadStatusProperty = BindableProperty.Create(nameof(LoadStatus), typeof(LoadMoreStatus), typeof(LoadMoreListView), LoadMoreStatus.StausDefault, propertyChanged: OnLoadStatusChanged);

        public LoadMoreStatus LoadStatus
        {
            get { return (LoadMoreStatus)GetValue(LoadStatusProperty); }
            set { SetValue(LoadStatusProperty, value); }
        }

        public static readonly BindableProperty FailHeightProperty = BindableProperty.Create(nameof(FailHeight), typeof(int), typeof(LoadMoreListView), 300);

        public int FailHeight
        {
            get { return (int)GetValue(FailHeightProperty); }
            set { SetValue(LoadStatusProperty, value); }
        }
        static void OnLoadStatusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var lv = (LoadMoreListView)bindable;
            lv.NotifyLoadStatus((LoadMoreStatus)newValue);
        }
        public void NotifyLoadStatus(LoadMoreStatus loadStatus)
        {
            switch (loadStatus)
            {
                case LoadMoreStatus.StausLoading:
                    this.Footer = loadingPage;
                    break;
                case LoadMoreStatus.StausNodata:
                    this.Footer = nodataPage;
                    break;
                case LoadMoreStatus.StausFail:
                    this.Footer = failPage;
                    break;
                case LoadMoreStatus.StausEnd:
                    this.Footer = endPage;
                    break;
                case LoadMoreStatus.StausError:
                    this.Footer = errorPage;
                    break;
                case LoadMoreStatus.StausNologin:
                    this.Footer = nologinPage;
                    break;
                default:
                    this.Footer = loadingPage;
                    break;
            }
        }        
    }
}
