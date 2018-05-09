using MvvmHelpers;
using Newtonsoft.Json;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class NewsComments
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
        public NewsComments ParentComment { get; set; }
        public string DateDisplay { get { return DateAdded.Format(); } }        
        public bool IsDelete { get; set; }
        public bool IsLoginUser
        {
            get
            {
                if (!UserTokenSettings.Current.HasExpiresIn() && CommentID > 0)
                {
                    return UserGuid.Equals(UserSettings.Current.UserId);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
