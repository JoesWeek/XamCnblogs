using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xam.Plugin.WebView.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamCnblogs.iOS.Renderers;
using XamCnblogs.UI.Controls;

[assembly: ExportRenderer(typeof(HomeTabbedPage), typeof(HomeTabbedPageRenderer))]
namespace XamCnblogs.iOS.Renderers
{
    public class HomeTabbedPageRenderer : TabbedRenderer
    {
        public HomeTabbedPageRenderer() : base()
        {
        }
        public override UIViewController SelectedViewController
        {
            get => base.SelectedViewController;
            set
            {
                base.SelectedViewController = value;
                (Element as HomeTabbedPage).Title = this.Tabbed.CurrentPage.Title;
            }
        }
    }
}