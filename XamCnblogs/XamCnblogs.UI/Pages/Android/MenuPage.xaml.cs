using System.Threading.Tasks;

using Xamarin.Forms;

namespace XamCnblogs.UI.Pages.Android
{
    public partial class MenuPage : ContentPage
    {
        RootPage root;
        public MenuPage(RootPage root)
        {
            this.root = root;
            InitializeComponent();

            NavView.NavigationItemSelected += async (sender, e) =>
            {
                this.root.IsPresented = false;

                await Task.Delay(225);
                await this.root.NavigateAsync(e.Index);
            };
        }
    }
}