
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;

namespace XamCnblogs.UI.Pages.About
{
    public partial class SettingWeibaPage : ContentPage
    {
        public SettingWeibaPage()
        {
            InitializeComponent();
            var cancel = new ToolbarItem
            {
                Text = "默认小尾巴",
                Command = new Command(() =>
                {
                    editorContent.Text = AboutSettings.DefaultWeibaContent;
                })
            };
            ToolbarItems.Add(cancel);
            editorContent.Text = AboutSettings.Current.WeibaContent;

            editorContent.TextChanged += (object sender, TextChangedEventArgs e) =>
            {
                AboutSettings.Current.WeibaContent = e.NewTextValue;
            };
        }
    }
}