using MvvmHelpers;
using Newtonsoft.Json;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class StatusesComments
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int StatusId { get; set; }
        public string UserAlias { get; set; }
        public string UserDisplayName { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserIconUrl { get; set; }
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
        public string DateDisplay { get { return DateAdded.Format(); } }
        public bool IsLoginUser
        {
            get
            {
                if (!UserTokenSettings.Current.HasExpiresIn() && Id > 0)
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
