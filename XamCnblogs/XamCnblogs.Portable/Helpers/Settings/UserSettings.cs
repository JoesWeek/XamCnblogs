using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class UserSettings : INotifyPropertyChanged
    {
        static UserSettings settings;
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static UserSettings Current
        {
            get { return settings ?? (settings = new UserSettings()); }
        }
        #region User
        const string UserIdKey = "user_id";
        public Guid UserId
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, Guid.Empty); }
            set
            {
                if (AppSettings.AddOrUpdateValue(UserIdKey, value))
                    OnPropertyChanged();
            }
        }
        const string SpaceUserIdKey = "space_user_id";
        public int SpaceUserId
        {
            get { return AppSettings.GetValueOrDefault(SpaceUserIdKey, 0); }
            set
            {
                if (AppSettings.AddOrUpdateValue(SpaceUserIdKey, value))
                    OnPropertyChanged();
            }
        }
        const string BlogIdKey = "blog_id";
        public int BlogId
        {
            get { return AppSettings.GetValueOrDefault(BlogIdKey, 0); }
            set
            {
                if (AppSettings.AddOrUpdateValue(BlogIdKey, value))
                    OnPropertyChanged();
            }
        }
        const string DisplayNameKey = "display_name";
        public string DisplayName
        {
            get { return AppSettings.GetValueOrDefault(DisplayNameKey, string.Empty); }
            set
            {
                if (AppSettings.AddOrUpdateValue(DisplayNameKey, value))
                    OnPropertyChanged();
            }
        }
        const string AvatarKey = "avatar";
        public string Avatar
        {
            get { return AppSettings.GetValueOrDefault(AvatarKey, string.Empty); }
            set
            {
                if (AppSettings.AddOrUpdateValue(AvatarKey, value))
                    OnPropertyChanged();
            }
        }
        const string FaceKey = "face";
        public string Face
        {
            get { return AppSettings.GetValueOrDefault(FaceKey, string.Empty); }
            set
            {
                if (AppSettings.AddOrUpdateValue(FaceKey, value))
                    OnPropertyChanged();
            }
        }
        const string SeniorityKey = "seniority";
        public string Seniority
        {
            get { return AppSettings.GetValueOrDefault(SeniorityKey, string.Empty); }
            set
            {
                if (AppSettings.AddOrUpdateValue(SeniorityKey, value))
                    OnPropertyChanged();
            }
        }
        const string BlogAppKey = "blog_app";
        public string BlogApp
        {
            get { return AppSettings.GetValueOrDefault(BlogAppKey, string.Empty); }
            set
            {
                if (AppSettings.AddOrUpdateValue(BlogAppKey, value))
                    OnPropertyChanged();
            }
        }
        const string ScoreKey = "score";
        public int Score
        {
            get { return AppSettings.GetValueOrDefault(ScoreKey, 0); }
            set
            {
                if (AppSettings.AddOrUpdateValue(ScoreKey, value))
                    OnPropertyChanged();
            }
        }
        public void UpdateUser(User user)
        {
            Avatar = user.Avatar;
            BlogApp = user.BlogApp;
            BlogId = user.BlogId;
            DisplayName = user.DisplayName;
            Face = user.Face;
            Score = user.Score;
            Seniority = user.Seniority;
            SpaceUserId = user.SpaceUserId;
            UserId = user.UserId;
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
