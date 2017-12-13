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
    public class AnswersDetailsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<AnswersComment> AnswersComment { get; } = new ObservableRangeCollection<AnswersComment>();
        QuestionsAnswers answers;
        public DateTime NextRefreshTime { get; set; }

        AnswersDetailsModel answersDetails;
        public AnswersDetailsModel AnswersDetails
        {
            get { return answersDetails; }
            set { SetProperty(ref answersDetails, value); }
        }
        LoadMoreStatus loadStatus;
        public LoadMoreStatus LoadStatus
        {
            get { return loadStatus; }
            set { SetProperty(ref loadStatus, value); }
        }

        public AnswersDetailsViewModel(QuestionsAnswers answers)
        {
            this.answers = answers;
            Title = answers.UserName + "的回答";
            NextRefreshTime = DateTime.Now.AddMinutes(15);
            AnswersDetails = new AnswersDetailsModel();
        }
        public void ExecuteAnswersDetails()
        {
            AnswersDetails.UserName = answers.AnswerUserInfo.UserName;
            AnswersDetails.IconDisplay = answers.AnswerUserInfo.IconDisplay;
            AnswersDetails.UserDisplay = answers.UserDisplay;
            AnswersDetails.IsBest = answers.IsBest;
            AnswersDetails.Answer = answers.Answer;
        }
        ICommand refreshCommand;
        public ICommand RefreshCommand =>
            refreshCommand ?? (refreshCommand = new Command(async () =>
            {
                try
                {
                    IsBusy = true;
                    NextRefreshTime = DateTime.Now.AddMinutes(15);

                    var result = await StoreManager.AnswersCommentService.GetCommentAsync(answers.AnswerID);
                    if (result.Success)
                    {
                        var comments = JsonConvert.DeserializeObject<List<AnswersComment>>(result.Message.ToString());
                        if (comments.Count > 0)
                        {
                            if (AnswersComment.Count > 0)
                                AnswersComment.Clear();
                            AnswersComment.AddRange(comments);
                            LoadStatus = LoadMoreStatus.StausEnd;
                        }
                        else
                        {
                            LoadStatus = LoadMoreStatus.StausNodata;
                        }
                    }
                    else
                    {
                        LoadStatus = LoadMoreStatus.StausError;
                        if (AnswersComment.Count > 0)
                            AnswersComment.Clear();
                    }
                    CanLoadMore = false;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    IsBusy = false;
                }
            }));

        public class AnswersDetailsModel : BaseViewModel
        {
            string userName;
            public string UserName
            {
                get { return userName; }
                set { SetProperty(ref userName, value); }
            }
            string userDisplay;
            public string UserDisplay
            {
                get { return userDisplay; }
                set { SetProperty(ref userDisplay, value); }
            }
            string iconDisplay;
            public string IconDisplay
            {
                get { return iconDisplay; }
                set { SetProperty(ref iconDisplay, value); }
            }
            string diggDisplay;
            public string DiggDisplay
            {
                get { return diggDisplay; }
                set { SetProperty(ref diggDisplay, value); }
            }
            string answer;
            public string Answer
            {
                get { return answer; }
                set { SetProperty(ref answer, value); }
            }
            bool isBest;
            public bool IsBest
            {
                get { return isBest; }
                set { SetProperty(ref isBest, value); }
            }
        }
    }
}
