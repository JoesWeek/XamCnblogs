using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class Bookmarks : BaseViewModel
    {
        private int wzLinkId;
        public int WzLinkId
        {
            get { return wzLinkId; }
            set { SetProperty(ref wzLinkId, value); }
        }
        private string title;
        public new string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private string linkUrl;
        public string LinkUrl
        {
            get { return linkUrl; }
            set { SetProperty(ref linkUrl, value); }
        }
        private string summary;
        public string Summary
        {
            get { return summary; }
            set { SetProperty(ref summary, value); }
        }
        private DateTime dateAdded;
        public DateTime DateAdded
        {
            get { return dateAdded; }
            set { SetProperty(ref dateAdded, value); }
        }
        private List<string> tags;
        public List<string> Tags
        {
            get { return tags; }
            set { SetProperty(ref tags, value); }
        }
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
                    tag = tag == null ? tag : t.TrimEnd(',');
                }
                return tag;
            }
            set { SetProperty(ref tag, value); }
        }
        public bool FromCNBlogs { get; set; }
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.Format(); } }

        private bool isDelete;
        [JsonIgnore]
        public bool IsDelete
        {
            get { return isDelete; }
            set { SetProperty(ref isDelete, value); }
        }
    }
}
