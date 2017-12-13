using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.Portable.ViewModel
{
    public class StatusesViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Statuses> Statuses { get; } = new ObservableRangeCollection<Statuses>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int position = 0;
        public StatusesViewModel(int position)
        {
            this.position = position;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
            ////判断有没有登录
            //if (position > 0 && Settings.Current.IsLoggedIn)
            //{
            //    LoadStatus = LoadMoreStatus.StausNologin;
            //}
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    CanLoadMore = false;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);
                    pageIndex = 1;
                    //if (position > 0 && Settings.Current.IsLoggedIn)
                    //{
                    //    //判断有没有登录
                    //    LoadStatus = LoadMoreStatus.StausNologin;
                    //    if (Statuses.Count > 0)
                    //        Statuses.Clear();
                    //}
                    //else
                    //{
                    //    await ExecuteRefreshCommandAsync();
                    //}
                }
                catch (Exception)
                {
                    if (Statuses.Count > 0)
                        Statuses.Clear();
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
            var result = await StoreManager.StatusesService.GetStatusesAsync(position, pageIndex);
            if (result.Success)
            {
                var statuses = JsonConvert.DeserializeObject<List<Statuses>>(result.Message.ToString());
                if (statuses.Count > 0)
                {
                    if (pageIndex == 1 && Statuses.Count > 0)
                        Statuses.Clear();
                    Statuses.AddRange(statuses);
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
