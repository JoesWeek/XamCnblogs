using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Microsoft.AppCenter.Crashes;

namespace XamCnblogs.Portable.ViewModel
{
    public class StatusesViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Statuses> Statuses { get; } = new ObservableRangeCollection<Statuses>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;
        private int position = 0;
        public StatusesViewModel(int position = 0)
        {
            this.position = position;
            CanLoadMore = false;
            //判断有没有登录
            if (position > 0 && UserTokenSettings.Current.HasExpiresIn())
            {
                LoadStatus = LoadMoreStatus.StausNologin;
            }
        }
        public async void GetClientStatusesAsync()
        {
            if (position == 0)
            {
                Statuses.AddRange(await SqliteUtil.Current.QueryStatuses(pageSize));
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
                    if (position > 0 && UserTokenSettings.Current.HasExpiresIn())
                    {
                        //判断有没有登录
                        LoadStatus = LoadMoreStatus.StausNologin;
                        Statuses.Clear();
                    }
                    else
                    {
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
            var result = await StoreManager.StatusesService.GetStatusesAsync(position, pageIndex, pageSize);
            if (result.Success)
            {
                var statuses = JsonConvert.DeserializeObject<List<Statuses>>(result.Message.ToString());
                if (statuses.Count > 0)
                {
                    if (pageIndex == 1 && Statuses.Count > 0)
                        Statuses.Clear();
                    Statuses.AddRange(statuses);

                    if (position == 0)
                    {
                        await SqliteUtil.Current.UpdateStatuses(statuses);
                    }
                    pageIndex++;
                    if (Statuses.Count >= pageSize)
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

        public async Task<bool> ExecuteStatusesEditCommandAsync(Statuses statuses)
        {
            var result = await StoreManager.StatusesService.EditStatusesAsync(statuses);
            if (result.Success)
            {
                Toast.SendToast(statuses.Id > 0 ? "修改闪存成功" : "发布成功");
            }
            else
            {
                Crashes.TrackError(new Exception() { Source = result.Message });
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
        public void EditStatuses(Statuses statuses)
        {
            var book = Statuses.Where(b => b.Id == statuses.Id).FirstOrDefault();
            if (book == null)
            {
                if (position == 0 || position == 2)
                {
                    Statuses.Insert(0, statuses);
                }
            }
            else
            {
                var index = Statuses.IndexOf(book);
                Statuses[index] = statuses;
            }
            if (LoadStatus == LoadMoreStatus.StausNodata)
                LoadStatus = LoadMoreStatus.StausEnd;
        }
        ICommand deleteCommand;
        public ICommand DeleteCommand =>
            deleteCommand ?? (deleteCommand = new Command<Statuses>(async (statuses) =>
            {
                var index = Statuses.IndexOf(statuses);
                if (!Statuses[index].IsDelete)
                {
                    Statuses[index].IsDelete = true;
                    var result = await StoreManager.StatusesService.DeleteStatusesAsync(statuses.Id);
                    if (result.Success)
                    {
                        await Task.Delay(1000);
                        index = Statuses.IndexOf(statuses);
                        Statuses.RemoveAt(index);
                        if (Statuses.Count == 0)
                            LoadStatus = LoadMoreStatus.StausNodata;
                    }
                    else
                    {
                        Crashes.TrackError(new Exception() { Source = result.Message });
                        index = Statuses.IndexOf(statuses);
                        Statuses[index].IsDelete = false;
                        Toast.SendToast("删除失败");
                    }
                }
            }));
    }
}
