using Newtonsoft.Json;
using SQLite;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class News
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int TopicId { get; set; }
        public string TopicIcon { get; set; }
        public int ViewCount { get; set; }
        public int CommentCount { get; set; }
        public int DiggCount { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsHot { get; set; }
        public bool IsRecommend { get; set; }
        public string Body { get; set; }
        [Ignore]
        public string DateDisplay { get { return DateAdded.Format(); } }
        [Ignore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + CommentCount + " 评论 · " + ViewCount + " 阅读";
            }
        }
    }
}
