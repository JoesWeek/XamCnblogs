using Humanizer;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamCnblogs.Portable.Model
{
    public class StatusesComments : BaseViewModel
    {
        public int Id { get; set; }
        private string content;
        public string Content
        {
            get { return content; }
            set { SetProperty(ref content, value); }
        }
        public int StatusId { get; set; }
        public string UserAlias { get; set; }
        public string UserDisplayName { get; set; }        
        public DateTime DateAdded { get; set; }
        public string UserIconUrl { get; set; }
        public int UserId { get; set; }
        public Guid UserGuid { get; set; }
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
