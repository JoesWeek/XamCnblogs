using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.Portable.Services;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.AppCenter.Crashes;

namespace XamCnblogs.Portable.ViewModel
{
    public class NewsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<News> News { get; } = new ObservableRangeCollection<News>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int position = 1;
        private int pageSize = 20;
        public NewsViewModel(int position)
        {
            this.position = position;
            CanLoadMore = false;
        }
        public async void GetClientNewsAsync()
        {
            if (position == 0)
            {
                News.AddRange(await SqliteUtil.Current.QueryNews(pageSize));
            }
            else if (position == 1)
            {
                News.AddRange(await SqliteUtil.Current.QueryNewsByRecommend(pageSize));
            }
            else if (position == 2)
            {
                News.AddRange(await SqliteUtil.Current.QueryNewsByWorkHot(pageSize, GetMondayDate(DateTime.Now)));
            }
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    IsBusy = true;
                    CanLoadMore = false;
                    pageIndex = 1;
                    await ExecuteRefreshCommandAsync();
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    LoadStatus = LoadMoreStatus.StausFail;
                }
                finally
                {
                    IsBusy = false;
                }
            }));


        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
        ICommand loadMoreCommand;
        public ICommand LoadMoreCommand =>
            loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
            {
                try
                {
                    LoadStatus = LoadMoreStatus.StausLoading;

                    await ExecuteRefreshCommandAsync();
                }
                catch (Exception)
                {
                    LoadStatus = LoadMoreStatus.StausError;
                }
            }));
        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.NewsService.GetNewsAsync(position, pageIndex, pageSize);
            if (result.Success)
            {
                var news = JsonConvert.DeserializeObject<List<News>>(result.Message.ToString());
                if (news.Count > 0)
                {
                    if (pageIndex == 1 && News.Count > 0)
                        News.Clear();
                    News.AddRange(news);
                    switch (position)
                    {
                        case 1:
                            news.ForEach(s => s.IsRecommend = true);
                            break;
                        case 2:
                            news.ForEach(s => s.IsHot = true);
                            break;
                    }
                    await SqliteUtil.Current.UpdateNews(news);
                    pageIndex++;
                    LoadStatus = LoadMoreStatus.StausDefault;
                    CanLoadMore = true;
                }
                else
                {
                    CanLoadMore = false;
                    LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
                }
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }
        public DateTime GetMondayDate(DateTime someDate)
        {
            int i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;
            TimeSpan ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }
    }
}
