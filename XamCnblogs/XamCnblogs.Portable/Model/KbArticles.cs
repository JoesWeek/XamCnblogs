using Newtonsoft.Json;
using SQLite;
using System;
using XamCnblogs.Portable.Helpers;

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
        public string DateDisplay { get { return DateAdded.Format(); } }
        [Ignore]
        public string DiggValue
        {
            get
            {
                return DiggCount + " 推荐 · " + ViewCount + " 阅读";
            }
        }
    }
}
