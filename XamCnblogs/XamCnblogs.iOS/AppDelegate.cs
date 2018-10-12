using FFImageLoading.Forms.Platform;
using FormsToolkit.iOS;
using Foundation;
using Naxam.Controls.Platform.iOS;
using Rg.Plugins.Popup;
using UIKit;
using Xam.Plugin.WebView.iOS;

namespace XamCnblogs.iOS {
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Popup.Init();
            FormsWebViewRenderer.Initialize();
            Toolkit.Init();
            TopTabbedRenderer.Init();
            CachedImageRenderer.Init();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new XamCnblogs.UI.App());

            return base.FinishedLaunching(app, options);
        }
    }
}
