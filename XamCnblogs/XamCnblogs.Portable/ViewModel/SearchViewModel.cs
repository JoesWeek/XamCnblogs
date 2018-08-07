using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Search> Searchs { get; } = new ObservableRangeCollection<Search>();
        private int pageIndex = 1;
        private int pageSize = 20;
        private int position = 0;

        public SearchViewModel(int position)
        {
            this.position = position;
            LoadStatus = LoadMoreStatus.StausDefault;
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }
        string searchValue;
        public string SearchValue
        {
            get { return searchValue; }
            set { SetProperty(ref searchValue, value); }
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    if (SearchValue != null && SearchValue != "")
                    {
                        IsBusy = true;
                        await ExecuteRefreshCommandAsync();
                    }
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

        ICommand loadMoreCommand;
        public ICommand LoadMoreCommand => loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
            {
                try
                {
                    if (SearchValue != null && SearchValue != "")
                    {
                        await ExecuteRefreshCommandAsync();
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausDefault;
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                    LoadStatus = LoadMoreStatus.StausError;
                }
            }));

        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.SearchService.GetSearchAsync(position, SearchValue, pageIndex, pageSize);
            if (result.Success)
            {
                var articles = JsonConvert.DeserializeObject<List<Search>>(result.Message.ToString());
                if (articles.Count > 0)
                {
                    if (pageIndex == 1 && Searchs.Count > 0)
                        Searchs.Clear();
                    Searchs.AddRange(articles);
                    pageIndex++;
                    if (Searchs.Count >= pageSize)
                    {
                        LoadStatus = LoadMoreStatus.StausDefault;
                        CanLoadMore = true;
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausEnd;
                        CanLoadMore = false;
                    }
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

        public async Task ExecuteSearchCommandAsync(string value)
        {
            if (value != SearchValue)
            {
                searchValue = value;
                pageIndex = 1;
                CanLoadMore = false;
                if (searchValue != "")
                {
                    await ExecuteRefreshCommandAsync();
                }
                else
                {
                    if (Searchs.Count > 0)
                        Searchs.Clear();
                    LoadStatus = LoadMoreStatus.StausDefault;
                }
            }
        }
    }
}
