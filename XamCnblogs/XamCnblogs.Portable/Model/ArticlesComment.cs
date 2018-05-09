using Newtonsoft.Json;
using System;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.Portable.Model
{
    public class ArticlesComments
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public string AuthorUrl { get; set; }

        public string FaceUrl { get; set; }

        public int Floor { get; set; }

        public DateTime DateAdded { get; set; }
        
        public string DateDisplay { get { return DateAdded.Format(); } }
        [JsonIgnore]
        public string FaceUrlDisplay { get { return FaceUrl == "" ? "https://pic.cnblogs.com/face/sample_face.gif" : FaceUrl; } }
    }
}
