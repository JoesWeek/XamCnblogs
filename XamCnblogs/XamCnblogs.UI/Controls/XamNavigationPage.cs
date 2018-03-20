using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;

namespace XamCnblogs.UI.Controls
{
    public class XamNavigationPage : NavigationPage
    {
        public XamNavigationPage(Page root) : base(root)
        {
            Init();
            Title = root.Title;
            Icon = root.Icon;
        }

        public XamNavigationPage()
        {
            Init();
        }

        void Init()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                BarBackgroundColor = Color.FromHex("FAFAFA");
            }
            else
            {
                BarBackgroundColor = (Color)Application.Current.Resources["Primary"];
                BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            }
        }
    }
}
