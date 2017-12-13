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

namespace XamCnblogs.Portable.ViewModel
{
    public class NewsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<News> News { get; } = new ObservableRangeCollection<News>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int position = 1;
        public NewsViewModel(int position)
        {
            this.position = position;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    await Task.Run(async () =>
                    {
                        CanLoadMore = false;
                        NextRefreshTime = DateTime.Now.AddMinutes(15);
                        pageIndex = 1;
                        await ExecuteRefreshCommandAsync();
                    });
                }
                catch (Exception)
                {
                    if (News.Count > 0)
                        News.Clear();
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
            var result = await StoreManager.NewsService.GetNewsAsync(position,pageIndex);
            if (result.Success)
            {
                var news = JsonConvert.DeserializeObject<List<News>>(result.Message.ToString());
                if (news.Count > 0)
                {
                    if (pageIndex == 1 && News.Count > 0)
                        News.Clear();
                    News.AddRange(news);
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
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }

    }
}
