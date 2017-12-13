using XamCnblogs.Portable.Helpers;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Model
{
    public class Questions
    {
        public int Qid { get; set; }
        public int DealFlag { get; set; }
        public int ViewCount { get; set; }
        public string Summary { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public string Supply { get; set; }
        public string ConvertedContent { get; set; }
        public int FormatType { get; set; }
        public string Tags { get; set; }
        public int AnswerCount { get; set; }
        public int UserId { get; set; }
        public int Award { get; set; }
        public int DiggCount { get; set; }
        public int BuryCount { get; set; }
        public bool IsBest { get; set; }
        public string AnswerUserId { get; set; }
        public int Flags { get; set; }
        public string DateEnded { get; set; }
        public int UserInfoID { get; set; }
        public QuestionUserInfo QuestionUserInfo { get; set; }
        [JsonIgnore]
        public string DateDisplay => DateAdded.ToUniversalTime().Humanize();
        [JsonIgnore]
        public string DiggValue => AnswerCount + " 回答 · " + ViewCount + " 阅读";
        [JsonIgnore]
        public string TagsDisplay => Tags == null ? "" : Tags.Replace(',', ' ');
    }
    public class QuestionUserInfo
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string UserEmail { get; set; }
        public string IconName { get; set; }
        public string Alias { get; set; }
        public int QScore { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserWealth { get; set; }
        public bool IsActive { get; set; }
        public Guid UCUserID { get; set; }
        [JsonIgnore]
        public string IconDisplay { get { return "https://pic.cnblogs.com/face/" + IconName; } }
    }
}
