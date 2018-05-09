using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamCnblogs.Portable.Interfaces;

namespace XamCnblogs.UI.Controls
{
    public class XamNavigationPage : NavigationPage
    {
        // 首次按下返回键时间戳
        private DateTime firstBackPressedTime = DateTime.MinValue;
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

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android && this.RootPage == this.CurrentPage)
            {
                if (firstBackPressedTime == DateTime.MinValue || firstBackPressedTime.AddSeconds(3) < DateTime.Now)
                {
                    DependencyService.Get<IToast>().SendToast("再按一次退出程序");
                    firstBackPressedTime = DateTime.Now;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return base.OnBackButtonPressed();
        }
    }
}
