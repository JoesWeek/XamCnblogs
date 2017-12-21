using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamCnblogs.Portable.Helpers;
using XamCnblogs.Portable.Model;
using XamCnblogs.UI.Pages.Account;

namespace XamCnblogs.UI.Cells
{
    public partial class BookmarksItemCell : ContentView
    {
        public BookmarksItemCell()
        {
            InitializeComponent();
            DeleteButton.ActivityIndicatorClick += async (sender, e) =>
            {
                DeleteButton.IsRunning = true;
                var id = e.ActionID;
                await Task.Delay(3000);
                DeleteButton.IsRunning = false;
            };
        }
        async void OnEdit(object sender, EventArgs args)
        {
            await NavigationService.PushAsync(Navigation, new BookmarksEditPage(new Bookmarks()));
        }
    }
}