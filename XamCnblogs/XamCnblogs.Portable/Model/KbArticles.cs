using XamCnblogs.Portable.Helpers;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace XamCnblogs.Portable.Model
{
    public class KbArticles
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public DateTime DateAdded { get; set; }
        public int ViewCount { get; set; }
        public int DiggCount { get; set; }
        public string Body { get; set; }
        [Ignore]
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [Ignore]
        [JsonIgnore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + ViewCount + " 阅读";
            }
        }
        [Ignore]
        [JsonIgnore]
        public string BodyDisplay
        {
            get
            {
                return HtmlTemplate.ReplaceHtml(Body, false);
            }
        }
    }
}
