using FFImageLoading.Transformations;
using FormsToolkit;
using Plugin.Messaging;
using System;

using Xamarin.Forms;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Interfaces;
using XamCnblogs.UI.Pages.About;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            Title = "我";
            Icon = "menu_avatar.png";

            ffimageloading.Transformations.Add(new CircleTransformation());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateUser();
        }
        void UpdateUser()
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                this.UserName.IsVisible = false;
                this.UserSeniority.IsVisible = false;
                this.LogoutLayout.IsVisible = false;
                this.Login.IsVisible = true;

                this.ffimageloading.Source = "avatar_placeholder.png";
                this.UserName.Text = "";
                this.UserSeniority.Text = "";
            }
            else
            {
                this.UserName.IsVisible = true;
                this.UserSeniority.IsVisible = true;
                this.LogoutLayout.IsVisible = true;
                this.Login.IsVisible = false;

                this.ffimageloading.Source = UserSettings.Current.Avatar;
                this.UserName.Text = UserSettings.Current.DisplayName;
                this.UserSeniority.Text = "园龄：" + UserSettings.Current.Seniority;
            }
        }
        void OnLogin(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
        }
        void OnLogout(object sender, EventArgs args)
        {
            //注销登录
            if (!UserTokenSettings.Current.HasExpiresIn())
            {
                UserSettings.Current.UpdateUser(new Portable.Model.User());
                UserTokenSettings.Current.UpdateUserToken(new Portable.Model.Token() { ExpiresIn = 0 });

                UpdateUser();
            }
        }
        void OnBlog(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                Navigation.PushAsync(new ArticlesPage(UserSettings.Current.BlogApp));
            }
        }
        void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
            else
            {
                Navigation.PushAsync(new BookmarksPage());
            }
        }
        void OnSetting(object sender, EventArgs args)
        {
            Navigation.PushAsync(new SettingPage());
        }
        void OnAbout(object sender, EventArgs args)
        {
            Navigation.PushAsync(new AboutPage());
        }
        void OnEmail(object sender, EventArgs args)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;
            if (emailMessenger.CanSendEmail)
            {
                emailMessenger.SendEmail("476920650@qq.com", "来自 XamCnblogs - "+ DependencyService.Get<IVersionName>().GetVersionName() + " 的客户端反馈", "");
            }
            else
            {
                DependencyService.Get<IToast>().SendToast("系统中没有安装邮件客户端");
            }
        }
    }
}