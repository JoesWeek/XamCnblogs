using MvvmHelpers;
using Newtonsoft.Json;
using SQLite;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class Questions
    {
        [PrimaryKey]
        public int Qid { get; set; }
        public string Title { get; set; }
        public int DealFlag { get; set; }
        public int ViewCount { get; set; }
        public string Summary { get; set; }
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
        [Ignore]
        public QuestionUserInfo QuestionUserInfo { get; set; }
        public int AdditionID { get; set; }
        [Ignore]
        public QuestionAddition Addition { get; set; }
        [Ignore]
        public string DateDisplay => DateAdded.Format();

        private string tag;
        [Ignore]
        public string TagsDisplay
        {
            get
            {
                return tag = Tags == null ? "" : Tags.Replace(',', ' ');
            }
            set { tag = value; }
        }
        [Ignore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + AnswerCount + " 回答 · " + ViewCount + " 阅读";
            }
        }
        [Ignore]
        public string ContentDisplay
        {
            get
            {
                if (Addition != null)
                {
                    Content += "<h2>问题补充：</h2>" + Addition.Content;
                }
                return Content;
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
        [Ignore]
        public string QScoreName { get { return Helpers.HtmlTemplate.GetScoreName(QScore); } }
        public DateTime DateAdded { get; set; }
        public int UserWealth { get; set; }
        public bool IsActive { get; set; }
        public Guid UCUserID { get; set; }

        [Ignore]
        public string IconDisplay
        {
            get
            {
                if (IconName.IndexOf("https://") > -1)
                    return IconName;
                return "https://pic.cnblogs.com/face/" + IconName;
            }
        }
    }
    public class QuestionAddition
    {
        [PrimaryKey]
        public int QID { get; set; }
        public string Content { get; set; }
        public string ConvertedContent { get; set; }
    }
}
