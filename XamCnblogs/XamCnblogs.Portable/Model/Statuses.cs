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
    public class Statuses
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsLucky { get; set; }
        public int CommentCount { get; set; }
        public string UserAlias { get; set; }
        public string UserDisplayName { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserIconUrl { get; set; }
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        [JsonIgnore]
        public string CommentValue
        {
            get
            {
                return CommentCount > 0 ? CommentCount.ToString() : "回复";
            }
        }
        [JsonIgnore]
        public string CommentDisplay
        {
            get
            {
                return CommentCount > 0 ? "共有 " + CommentCount + " 条回复" : "暂时没有回复";
            }
        }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [JsonIgnore]
        public List<StatusesComments> Comments { get; set; } = new List<StatusesComments>();
    }
}
