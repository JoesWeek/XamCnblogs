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
    public class BookmarksViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Bookmarks> Bookmarks { get; } = new ObservableRangeCollection<Bookmarks>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int pageSize = 20;
        public BookmarksViewModel()
        {
            NextRefreshTime = DateTime.Now.AddMinutes(15);
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
                    await Task.Run(async () =>
                    {
                        await ExecuteRefreshCommandAsync();
                    });
                }
                catch (Exception ex)
                {
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
        public ICommand LoadMoreCommand => loadMoreCommand ?? (loadMoreCommand = new Command(async () =>
        {
            try
            {
                await ExecuteRefreshCommandAsync();
            }
            catch (Exception)
            {
                LoadStatus = LoadMoreStatus.StausError;
            }
        }));
        async Task ExecuteRefreshCommandAsync()
        {
            var result = await StoreManager.BookmarksService.GetBookmarksAsync(pageIndex, pageSize);
            if (result.Success)
            {
                var articles = JsonConvert.DeserializeObject<List<Bookmarks>>(result.Message.ToString());
                if (articles.Count > 0)
                {
                    if (pageIndex == 1 && Bookmarks.Count > 0)
                        Bookmarks.Clear();
                    Bookmarks.AddRange(articles);
                    pageIndex++;
                    if (Bookmarks.Count >= pageSize)
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
                Toast.SendToast(result.Message.ToString());
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausEnd : LoadMoreStatus.StausNodata;
            }
        }

        public async Task<bool> ExecuteBookmarkEditCommandAsync(Bookmarks bookmarks)
        {
            var result = await StoreManager.BookmarksService.EditBookmarkAsync(bookmarks);
            if (!result.Success)
            {
                Toast.SendToast(result.Message.ToString());
            }
            return result.Success;
        }
    }
}
