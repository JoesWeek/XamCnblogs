using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamCnblogs.Portable.Model
{
    public class Bookmarks
    {
        public int WzLinkId { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string Summary { get; set; }
        public DateTime DateAdded { get; set; }
        public List<string> Tags { get; set; }
        private string tag;
        public string TagsDisplay
        {
            get
            {
                if (Tags != null)
                {
                    string t = null;
                    for (int i = 0; i < Tags.Count; i++)
                    {
                        t += Tags[i] + ",";
                    }
                    tag = t.TrimEnd(',');
                }
                return tag;
            }
            set { tag = value; }
        }
        public bool FromCNBlogs { get; set; }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
    }
}
