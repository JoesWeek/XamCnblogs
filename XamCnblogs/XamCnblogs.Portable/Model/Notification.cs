using System;
using System.Collections.Generic;
using System.Text;

namespace XamCnblogs.Portable.Model
{
    public class Notification
    {
        /// <summary>
        /// ID或者标识
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题或者说明
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 博文 articles
        /// 新闻 news
        /// 知识库 kbarticles
        /// 博问 questions
        /// 更新 update
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }
    }
}
