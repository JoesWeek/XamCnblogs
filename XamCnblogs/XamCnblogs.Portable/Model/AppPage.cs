using System;
using System.Collections.Generic;
using System.Text;

namespace XamCnblogs.Portable.Model
{
    public class DeepLinkPage
    {
        public AppPage Page { get; set; }
        public string Id { get; set; }
    }
    public enum AppPage
    {
        Articles,
        News,
        KbArticles,
        Statuses,
        Questions
    }
}
