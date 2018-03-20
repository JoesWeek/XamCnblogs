using XamCnblogs.Portable.Helpers;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using SQLite;

namespace XamCnblogs.Portable.Model
{
    public class Questions : BaseViewModel
    {
        [PrimaryKey]
        public int Qid { get; set; }
        private int dealFlag;
        public int DealFlag
        {
            get { return dealFlag; }
            set { SetProperty(ref dealFlag, value); }
        }
        private int viewCount;
        public int ViewCount
        {
            get { return viewCount; }
            set { SetProperty(ref viewCount, value); }
        }
        private string summary;
        public string Summary
        {
            get { return summary; }
            set { SetProperty(ref summary, value); }
        }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public string Supply { get; set; }
        public string ConvertedContent { get; set; }
        public int FormatType { get; set; }
        public string Tags { get; set; }
        private int answerCount;
        public int AnswerCount
        {
            get { return answerCount; }
            set { SetProperty(ref answerCount, value); }
        }
        public int UserId { get; set; }
        public int Award { get; set; }
        private int diggCount;
        public int DiggCount
        {
            get { return diggCount; }
            set { SetProperty(ref diggCount, value); }
        }
        private int buryCount;
        public int BuryCount
        {
            get { return buryCount; }
            set { SetProperty(ref buryCount, value); }
        }
        public bool IsBest { get; set; }
        public string AnswerUserId { get; set; }
        public int Flags { get; set; }
        public string DateEnded { get; set; }
        public int UserInfoID { get; set; }
        [Ignore]
        public QuestionUserInfo QuestionUserInfo { get; set; }
        public int AdditionID { get; set; }
        [Ignore]
        public QuestionAddition Addition { get; set; }
        [Ignore]
        [JsonIgnore]
        public string DateDisplay => DateAdded.ToUniversalTime().Humanize();
        [Ignore]
        [JsonIgnore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + AnswerCount + " 回答 · " + ViewCount + " 阅读";
            }
        }
        private string tag;
        [Ignore]
        [JsonIgnore]
        public string TagsDisplay
        {
            get
            {
                return tag = Tags == null ? "" : Tags.Replace(',', ' ');
            }
            set { SetProperty(ref tag, value); }
        }
        [Ignore]
        [JsonIgnore]
        public string ContentDisplay
        {
            get
            {
                if (Addition != null)
                {
                    Content += "<h2>问题补充：</h2>" + Addition.Content;
                }
                return HtmlTemplate.ReplaceHtml(Content, Tags);
            }
        }
    }
    public class QuestionUserInfo
    {
        [PrimaryKey]
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

        [Ignore]
        [JsonIgnore]
        public string IconDisplay { get { return "https://pic.cnblogs.com/face/" + IconName; } }
    }
    public class QuestionAddition
    {
        [PrimaryKey]
        public int QID { get; set; }
        public string Content { get; set; }
        public string ConvertedContent { get; set; }
    }
}
