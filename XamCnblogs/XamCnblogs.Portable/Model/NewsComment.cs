using Humanizer;
using MvvmHelpers;
using Newtonsoft.Json;
using System;

namespace XamCnblogs.Portable.Model
{
    public class NewsComments : BaseViewModel
    {
        public int CommentID { get; set; }
        public int ContentID { get; set; }
        private string commentContent;
        public string CommentContent
        {
            get { return commentContent; }
            set { SetProperty(ref commentContent, value); }
        }
        public Guid UserGuid { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FaceUrl { get; set; }
        public int Floor { get; set; }
        public DateTime DateAdded { get; set; }
        public int AgreeCount { get; set; }
        public int AntiCount { get; set; }
        public int ParentCommentID { get; set; }
        public NewsComments ParentComment { get; set; }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }

        private bool isDelete;
        [JsonIgnore]
        public bool IsDelete
        {
            get { return isDelete; }
            set { SetProperty(ref isDelete, value); }
        }
    }
}
