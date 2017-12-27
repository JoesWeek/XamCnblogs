using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class UserTokenSettings : INotifyPropertyChanged
    {
        static UserTokenSettings settings;
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static UserTokenSettings Current
        {
            get { return settings ?? (settings = new UserTokenSettings()); }
        }

        /// <summary>
        /// true已过期，false未过期
        /// </summary>
        public bool HasExpiresIn()
        {
            if (UserToken != null && UserTokenRefreshTime.AddSeconds(UserExpiresIn) > DateTime.Now)
            {
                return false;
            }
            else
            {
                UpdateUserToken(new Token());
                return true;
            }
        }
        const string UserTokenKey = "user_token";
        public string UserToken
        {
            get { return AppSettings.GetValueOrDefault(UserTokenKey, null); }
            set
            {
                if (AppSettings.AddOrUpdateValue(UserTokenKey, value))
                    OnPropertyChanged();
            }
        }

        const string UserExpiresInKey = "user_expires_in";
        public int UserExpiresIn
        {
            get { return AppSettings.GetValueOrDefault(UserExpiresInKey, 0); }
            set
            {
                if (AppSettings.AddOrUpdateValue(UserExpiresInKey, value))
                    OnPropertyChanged();
            }
        }

        const string UserTokenTypeKey = "user_token_type";
        public string UserTokenType
        {
            get { return AppSettings.GetValueOrDefault(UserTokenTypeKey, null); }
            set
            {
                if (AppSettings.AddOrUpdateValue(UserTokenTypeKey, value))
                    OnPropertyChanged();
            }
        }
        const string UserTokenRefreshTimeKey = "user_refresh_time";
        public DateTime UserTokenRefreshTime
        {
            get { return AppSettings.GetValueOrDefault(UserTokenRefreshTimeKey, DateTime.MinValue); }
            set
            {
                if (AppSettings.AddOrUpdateValue(UserTokenRefreshTimeKey, value))
                    OnPropertyChanged();
            }
        }
        const string UserRefreshTokenKey = "user_refresh_token";
        public string UserRefreshToken
        {
            get { return AppSettings.GetValueOrDefault(UserRefreshTokenKey, null); }
            set
            {
                if (AppSettings.AddOrUpdateValue(UserRefreshTokenKey, value))
                    OnPropertyChanged();
            }
        }
        public void UpdateUserToken(Token token)
        {
            UserToken = token.AccessToken;
            UserExpiresIn = token.ExpiresIn;
            UserTokenType = token.TokenType;
            UserRefreshToken = token.RefreshToken;
            UserTokenRefreshTime = token.RefreshTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
