using Humanizer;
using Newtonsoft.Json;
using SQLite;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class Articles
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string BlogApp { get; set; }
        public string Avatar { get; set; }
        public DateTime PostDate { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public int DiggCount { get; set; }
        public string Body { get; set; }
        [Ignore]
        [JsonIgnore]
        public string DateDisplay { get { return PostDate.ToUniversalTime().Humanize(true,null,new System.Globalization.CultureInfo("zh-CN")); } }
        [Ignore]
        [JsonIgnore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + CommentCount + " 评论 · " + ViewCount + " 阅读";
            }
        }
        [Ignore]
        [JsonIgnore]
        public string BodyDisplay
        {
            get
            {
                return HtmlTemplate.ReplaceHtml(Body);
            }
        }
    }
}
