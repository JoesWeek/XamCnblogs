using Plugin.CurrentActivity;
using System.IO;
using XamCnblogs.Droid.Helpers;
using XamCnblogs.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(HtmlTemplates))]
namespace XamCnblogs.Droid.Helpers
{
    public class HtmlTemplates : IHtmlTemplate
    {
        static string articlesTemplate;
        static string kbarticlesTemplate;
        static string newsTemplate;
        static string questionsTemplate;
        static string answersTemplate;
        static string statusesTemplate;
        public string GetArticlesTemplate()
        {
            if (articlesTemplate == null)
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                using (var stream = context.Assets.Open("articles.html"))
                {
                    StreamReader sr = new StreamReader(stream);
                    articlesTemplate = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            return articlesTemplate;
        }
        public string GetKbArticlesTemplate()
        {
            if (kbarticlesTemplate == null)
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                using (var stream = context.Assets.Open("kbarticles.html"))
                {
                    StreamReader sr = new StreamReader(stream);
                    kbarticlesTemplate = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            return kbarticlesTemplate;
        }
        public string GetNewsTemplate()
        {
            if (newsTemplate == null)
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                using (var stream = context.Assets.Open("news.html"))
                {
                    StreamReader sr = new StreamReader(stream);
                    newsTemplate = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            return newsTemplate;
        }
        public string GetQuestionsTemplate()
        {
            if (questionsTemplate == null)
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                using (var stream = context.Assets.Open("questions.html"))
                {
                    StreamReader sr = new StreamReader(stream);
                    questionsTemplate = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            return questionsTemplate;
        }
        public string GetAnswersTemplate()
        {
            if (answersTemplate == null)
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                using (var stream = context.Assets.Open("answers.html"))
                {
                    StreamReader sr = new StreamReader(stream);
                    answersTemplate = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            return answersTemplate;
        }
        public string GetStatusesTemplate()
        {
            if (statusesTemplate == null)
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                using (var stream = context.Assets.Open("statuses.html"))
                {
                    StreamReader sr = new StreamReader(stream);
                    statusesTemplate = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
            }
            return statusesTemplate;
        }
    }
}