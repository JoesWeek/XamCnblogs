using XamCnblogs.Portable.Helpers;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using SQLite;

namespace XamCnblogs.Portable.Model
{
    public class Statuses : BaseViewModel
    {
        [PrimaryKey]
        public int Id { get; set; }
        private string content;
        public string Content
        {
            get { return IsLucky ? content + "<img src='https://raw.githubusercontent.com/JoesWeek/XamCnblogs/master/XamCnblogs/XamCnblogs.Android/Assets/lucky-star.png' />" : content; }
            set { SetProperty(ref content, value); }
        }
        public bool IsPrivate { get; set; }
        public bool IsLucky { get; set; }
        public int CommentCount { get; set; }
        public string UserAlias { get; set; }
        public string UserDisplayName { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserIconUrl { get; set; }
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        [Ignore]
        [JsonIgnore]
        public string CommentValue
        {
            get
            {
                return CommentCount > 0 ? CommentCount.ToString() : "回复";
            }
        }
        [Ignore]
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [Ignore]
        [JsonIgnore]
        public List<StatusesComments> Comments { get; set; } = new List<StatusesComments>();

        private bool isDelete;
        [Ignore]
        [JsonIgnore]
        public bool IsDelete
        {
            get { return isDelete; }
            set { SetProperty(ref isDelete, value); }
        }
    }
}
