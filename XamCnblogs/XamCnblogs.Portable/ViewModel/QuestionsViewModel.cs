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
    public class QuestionsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Questions> Questions { get; } = new ObservableRangeCollection<Questions>();
        public DateTime NextRefreshTime { get; set; }
        private int pageIndex = 1;
        private int position = 1;
        public QuestionsViewModel(int position)
        {
            this.position = position;
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            CanLoadMore = false;
            //判断有没有登录
            if (position == 4 && UserTokenSettings.Current.HasExpiresIn())
            {
                LoadStatus = LoadMoreStatus.StausNologin;
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
                    if (position == 4 && UserTokenSettings.Current.HasExpiresIn())
                    {
                        //判断有没有登录
                        LoadStatus = LoadMoreStatus.StausNologin;
                        if (Questions.Count > 0)
                            Questions.Clear();
                    }
                    else
                    {
                        await ExecuteRefreshCommandAsync();
                    }
                }
                catch (Exception)
                {
                    if (Questions.Count > 0)
                        Questions.Clear();
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
            var result = await StoreManager.QuestionsService.GetQuestionsAsync(position, pageIndex);
            if (result.Success)
            {
                var questions = JsonConvert.DeserializeObject<List<Questions>>(result.Message.ToString());
                if (questions.Count > 0)
                {
                    if (pageIndex == 1 && Questions.Count > 0)
                        Questions.Clear();
                    Questions.AddRange(questions);
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
                Toast.SendToast(result.Message.ToString());
                LoadStatus = pageIndex > 1 ? LoadMoreStatus.StausError : LoadMoreStatus.StausFail;
            }
        }
    }
}
