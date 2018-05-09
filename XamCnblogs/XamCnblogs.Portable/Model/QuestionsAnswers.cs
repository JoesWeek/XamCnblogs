using MvvmHelpers;
using Newtonsoft.Json;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class QuestionsAnswers
    {
        public int Qid { get; set; }
        public int AnswerID { get; set; }
        public string Answer { get; set; }
        public string ConvertedContent { get; set; }
        public int FormatType { get; set; }
        public string UserName { get; set; }
        public bool IsBest { get; set; }
        public QuestionUserInfo AnswerUserInfo { get; set; }
        public string AnswerComments { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserID { get; set; }
        public int DiggCount { get; set; }
        public int Score { get; set; }
        public int BuryCount { get; set; }
        public int CommentCounts { get; set; }
        public string DateDisplay { get { return DateAdded.Format(); } }
        public bool IsLoginUser
        {
            get
            {
                if (!UserTokenSettings.Current.HasExpiresIn() && AnswerID > 0)
                {
                    return UserID.Equals(UserSettings.Current.SpaceUserId);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
