using Humanizer;
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

        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [JsonIgnore]
        public string UserDisplay { get { return HtmlTemplate.GetScoreName(AnswerUserInfo.QScore) + " · " + AnswerUserInfo.QScore + "园豆" + " · 回答于 " + DateDisplay; } }
        [JsonIgnore]
        public string CommentValue
        {
            get
            {
                return CommentCounts > 0 ? CommentCounts.ToString() : "回复";
            }
        }
        [JsonIgnore]
        public string AnswerDisplay
        {
            get
            {
                return HtmlTemplate.ReplaceHtml(Answer);
            }
        }
    }
}
