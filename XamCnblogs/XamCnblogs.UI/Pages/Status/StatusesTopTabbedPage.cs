using Naxam.Controls.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamCnblogs.UI.Pages.Status
{
    public class StatusesTopTabbedPage : TopTabbedPage
    {
        public StatusesTopTabbedPage()
        {
            BarTextColor = (Color)Application.Current.Resources["NavigationText"];
            BarIndicatorColor = (Color)Application.Current.Resources["Divider"];
            BarBackgroundColor = (Color)Application.Current.Resources["Primary"];

            Title = "闪存";
            Icon = "menu_statuses.png";
            
            this.Children.Add(new StatusesPage() { Title = "全站" });
            this.Children.Add(new StatusesPage(1) { Title = "关注" });
            this.Children.Add(new StatusesPage(2) { Title = "我的" });
            this.Children.Add(new StatusesPage(3) { Title = "我回应" });
            //this.Children.Add(new StatusesPage(4) { Title = "新回应" });
            //this.Children.Add(new StatusesPage(5) { Title = "提到我" });
            this.Children.Add(new StatusesPage(6) { Title = "回复我" });
        }
    }
}
