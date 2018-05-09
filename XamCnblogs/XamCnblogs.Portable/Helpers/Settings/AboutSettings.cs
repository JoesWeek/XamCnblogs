using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using XamCnblogs.Portable.Model;

namespace XamCnblogs.Portable.Helpers
{
    public class AboutSettings : INotifyPropertyChanged
    {
        static AboutSettings settings;
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static AboutSettings Current
        {
            get { return settings ?? (settings = new AboutSettings()); }
        }
        public static string DefaultWeibaContent = "来自 Cnblogs For Xamarin";

        const string WeibaToggledKey = "weiba_toggled";
        public bool WeibaToggled
        {
            get { return AppSettings.GetValueOrDefault(WeibaToggledKey, true); }
            set
            {
                if (AppSettings.AddOrUpdateValue(WeibaToggledKey, value))
                    OnPropertyChanged();
            }
        }
        const string WeibaContentKey = "weiba_content";
        public string WeibaContent
        {
            get { return AppSettings.GetValueOrDefault(WeibaContentKey, DefaultWeibaContent); }
            set
            {
                if (AppSettings.AddOrUpdateValue(WeibaContentKey, value))
                    OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
