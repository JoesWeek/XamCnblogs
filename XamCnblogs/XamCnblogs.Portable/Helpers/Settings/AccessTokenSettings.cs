using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class AccessTokenSettings : INotifyPropertyChanged
    {
        static AccessTokenSettings settings;
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static AccessTokenSettings Current
        {
            get { return settings ?? (settings = new AccessTokenSettings()); }
        }

        /// <summary>
        /// true已过期，false未过期
        /// </summary>
        public bool AccessTokenExpiresIn => TokenRefreshTime.AddSeconds(ExpiresIn) < DateTime.Now;

        const string AccessTokenKey = "access_token";
        public string AccessToken
        {
            get { return AppSettings.GetValueOrDefault(AccessTokenKey, null); }
            set
            {
                if (AppSettings.AddOrUpdateValue(AccessTokenKey, value))
                    OnPropertyChanged();
            }
        }

        const string ExpiresInKey = "expires_in";
        public int ExpiresIn
        {
            get { return AppSettings.GetValueOrDefault(ExpiresInKey, 0); }
            set
            {
                if (AppSettings.AddOrUpdateValue(ExpiresInKey, value))
                    OnPropertyChanged();
            }
        }

        const string TokenTypeKey = "token_type";
        public string TokenType
        {
            get { return AppSettings.GetValueOrDefault(TokenTypeKey, null); }
            set
            {
                if (AppSettings.AddOrUpdateValue(TokenTypeKey, value))
                    OnPropertyChanged();
            }
        }

        const string TokenRefreshTimeKey = "refresh_time";
        public DateTime TokenRefreshTime
        {
            get { return AppSettings.GetValueOrDefault(TokenRefreshTimeKey, DateTime.MinValue); }
            set
            {
                if (AppSettings.AddOrUpdateValue(TokenRefreshTimeKey, value))
                    OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        
        public static void UpdateToken(Token token)
        {
            AccessTokenSettings.Current.AccessToken = token.AccessToken;
            AccessTokenSettings.Current.ExpiresIn = token.ExpiresIn;
            AccessTokenSettings.Current.TokenType = token.TokenType;
            AccessTokenSettings.Current.TokenRefreshTime = DateTime.Now;
        }
    }
}
