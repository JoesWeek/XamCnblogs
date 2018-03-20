namespace XamCnblogs.Portable.Helpers
{
    public static class MessageKeys
    {
        public const string NavigateLogin = "navigate_login";
        public const string NavigateToken = "navigate_token";
        public const string NavigateAccount = "navigate_account";
    }
    public static class HtmlTemplate
    {
        static readonly string header = @"<html>
<head>
    <title>Cnblogs</title>
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0, maximum-scale=1.0,minimum-scale=1.0, user-scalable=no"" />
    <link rel=""stylesheet"" type=""text/css"" href=""default.css"" />
</head>
<body> 
        <div class=""content"">#content#</div>";
        static readonly string footer = @"</body></html>";
        public static string ReplaceHtml(string body, bool hasComment = true)
        {
            var content = header;
            if (hasComment)
            {
                content += @"<div class=""footer"">
                                        <div class=""line""></div>
                                        <div class=""comment"">所以评论</div>
                                        <div class=""line""></div>
                                    </div>";
            }
            content += footer;
            return content.Replace("#content#", body);
        }
        public static string ReplaceHtml(string body, string tags)
        {
            var content = header;
            if (tags != null)
            {
                content += @"<div class=""tags"">
                                        <img src=""ic_tab.png"" /> " + tags +
                                        "</div>";
            }
            content += @"<div class=""footer"">
                                    <div class=""line""></div>
                                    <div class=""comment"">所以回答</div>
                                    <div class=""line""></div>
                                </div>";
            content += footer;
            return content.Replace("#content#", body);
        }

        public static string GetScoreName(int score)
        {
            if (score > 100000)
            {
                return "大牛九级";
            }
            if (score > 50000)
            {
                return "牛人八级";
            }
            if (score > 20000)
            {
                return "高人七级";
            }
            if (score > 10000)
            {
                return "专家六级";
            }
            if (score > 5000)
            {
                return "大侠五级";
            }
            if (score > 2000)
            {
                return "老鸟四级";
            }
            if (score > 500)
            {
                return "小虾三级";
            }
            if (score > 200)
            {
                return "初学一级";
            }
            return "初学一级";
        }
    }

    public enum LoadMoreStatus
    {
        StausDefault = 0,
        StausLoading = 1,
        StausNodata = 2,
        StausFail = 3,
        StausEnd = 4,
        StausError = 5,
        StausNologin = 6
    }
}
