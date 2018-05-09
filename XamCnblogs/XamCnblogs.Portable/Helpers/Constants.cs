namespace XamCnblogs.Portable.Helpers
{
    public static class MessageKeys
    {
        public const string NavigateLogin = "navigate_login";
        public const string NavigateToken = "navigate_token";
        public const string NavigateAccount = "navigate_account";
        public const string NavigateNotification = "navigate_notification";
    }
    public static class HtmlTemplate
    {
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
        StausEnd = 3,
        StausFail = 4,
        StausError = 5,
        StausNologin = 6
    }
}
