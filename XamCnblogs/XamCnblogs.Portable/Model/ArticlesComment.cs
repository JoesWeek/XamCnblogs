using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [JsonIgnore]
        public string DateDisplay { get { return DateAdded.ToUniversalTime().Humanize(); } }
        [JsonIgnore]
        public string FaceUrlDisplay { get { return FaceUrl == "" ? "https://pic.cnblogs.com/face/sample_face.gif" : FaceUrl; } }
    }
}
