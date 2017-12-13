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
    public class NewsComment
    {
        public int CommentID { get; set; }
        public int ContentID { get; set; }
        public string CommentContent { get; set; }
        public Guid UserGuid { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FaceUrl { get; set; }
        public int Floor { get; set; }
        public DateTime DateAdded { get; set; }
        public int AgreeCount { get; set; }
        public int AntiCount { get; set; }
        public int ParentCommentID { get; set; }
        public NewsComment ParentComment { get; set; }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
    }
}
