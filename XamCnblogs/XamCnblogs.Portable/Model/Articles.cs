using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class Articles
    {
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
        [JsonIgnore]
        public string DateDisplay { get { return PostDate.ToUniversalTime().Humanize(); } }
        [JsonIgnore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + CommentCount + " 评论 · " + ViewCount + " 阅读";
            }
        }
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
