using Newtonsoft.Json;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class Search
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string UserAlias { get; set; } 
        public DateTime PublishTime { get; set; }
        public int VoteTimes { get; set; }
        public int ViewTimes { get; set; }
        public int CommentTimes { get; set; }
        public string Uri { get; set; }
        public string Id { get; set; }
        [JsonIgnore]
        public string DateDisplay { get { return PublishTime.Format(); } }
        [JsonIgnore]
        public string DiggValue
        {
            get
            {
                return VoteTimes + " 推荐 · " + CommentTimes + " 评论 · " + ViewTimes + " 阅读";
            }
        }
    }
}
