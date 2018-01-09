using XamCnblogs.Portable.Helpers;
using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;

namespace XamCnblogs.Portable.Model
{
    public class Statuses : BaseViewModel
    {
        public int Id { get; set; }
        private string content;
        public string Content
        {
            get { return IsLucky ? content + "<img src='https://github.com/JoesWeek/XamCnblogs/blob/master/XamCnblogs/XamCnblogs.Android/Assets/lucky-star.png' />" : content; }
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
        [JsonIgnore]
        public string CommentValue
        {
            get
            {
                return CommentCount > 0 ? CommentCount.ToString() : "回复";
            }
        }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [JsonIgnore]
        public List<StatusesComments> Comments { get; set; } = new List<StatusesComments>();

        private bool isDelete;
        [JsonIgnore]
        public bool IsDelete
        {
            get { return isDelete; }
            set { SetProperty(ref isDelete, value); }
        }
    }
}
