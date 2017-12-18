using FFImageLoading;
using FFImageLoading.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.UI.Pages.Account
{
    public partial class AccountPage : ContentPage
    {
        public AccountPage()
        {
            InitializeComponent();
            var cancel = new ToolbarItem
            {
                Text = "关闭",
                Command = new Command(async () =>
                {
                    await Navigation.PopModalAsync();
                })
            };
            ToolbarItems.Add(cancel);

            if (Device.Android == Device.RuntimePlatform)
                cancel.Icon = "toolbar_close.png";

            UpdateUser();
        }
        void UpdateUser()
        {
            if (UserSettings.Current.Avatar == "")
            {
                this.UserName.IsVisible = false;
                this.UserSeniority.IsVisible = false;
                this.LogoutLayout.IsVisible = false;
                this.Login.IsVisible = true;

                this.AvatarLayout.Source = "avatar_placeholder.png";
                this.UserName.Text = "";
                this.UserSeniority.Text = "";
            }
            else
            {
                this.UserName.IsVisible = true;
                this.UserSeniority.IsVisible = true;
                this.LogoutLayout.IsVisible = true;
                this.Login.IsVisible = false;

                this.AvatarLayout.Source = UserSettings.Current.Avatar;
                this.UserName.Text = UserSettings.Current.DisplayName;
                this.UserSeniority.Text = "园龄：" + UserSettings.Current.Seniority;
            }
        }
        void OnLogin(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                Navigation.PushAsync(new AuthorizePage());
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
                Navigation.PushAsync(new AuthorizePage());
            }
            else
            {
                Navigation.PushAsync(new ArticlesPage());
            }
        }
        void OnBookmarks(object sender, EventArgs args)
        {
            if (UserTokenSettings.Current.HasExpiresIn())
            {
                Navigation.PushAsync(new AuthorizePage());
            }
            else
            {
                Navigation.PushAsync(new BookmarksPage());
            }
        }
    }
}