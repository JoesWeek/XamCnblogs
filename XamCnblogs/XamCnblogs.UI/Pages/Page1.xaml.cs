using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamCnblogs.UI.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();

            Button.ActivityIndicatorClick += async (sender, e) =>
            {
                Button.IsRunning = true;
                var id = e.ActionID;
                await Task.Delay(3000);
                Button.IsRunning = false;
            };

        }
    }
}