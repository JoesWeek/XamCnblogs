using MvvmHelpers;
using SQLite;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class Statuses : BaseViewModel
    {
        [PrimaryKey]
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
        [Ignore]
        public string CommentValue
        {
            get
            {
                return CommentCount > 0 ? CommentCount.ToString() : "回复";
            }
        }
        [Ignore]
        public string DateDisplay { get { return DateAdded.Format(); } }

        private bool isDelete;
        [Ignore]
        public bool IsDelete
        {
            get { return isDelete; }
            set { SetProperty(ref isDelete, value); }
        }
    }
}
