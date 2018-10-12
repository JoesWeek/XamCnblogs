
using Android.App;
using Android.Content;
using Android.Widget;
using Com.Umeng.Socialize;
using Com.Umeng.Socialize.Bean;
using Com.Umeng.Socialize.Media;
using Com.Umeng.Socialize.Shareboard;
using Com.Umeng.Socialize.Utils;
using Java.Lang;

namespace XamCnblogs.Droid.Helpers {
    public class SharesWidget : Java.Lang.Object, IShareBoardlistener, IUMShareListener {
        private Activity context;
        private ShareAction shareAction;
        private UMWeb umWeb;
        public SharesWidget(Activity context) {
            this.context = context;

            shareAction = new ShareAction(context)
                                .SetDisplayList(SHARE_MEDIA.Weixin, SHARE_MEDIA.WeixinCircle, SHARE_MEDIA.WeixinFavorite, SHARE_MEDIA.Sina)
                                .AddButton("umeng_sharebutton_copy", "umeng_sharebutton_copy", "umeng_socialize_copy", "umeng_socialize_copy")
                                .AddButton("umeng_sharebutton_copyurl", "umeng_sharebutton_copyurl", "umeng_socialize_copyurl", "umeng_socialize_copyurl")
                                .SetShareboardclickCallback(this);
        }

        public void Onclick(SnsPlatform snsPlatform, SHARE_MEDIA media) {
            if (snsPlatform.MShowWord.Equals("umeng_sharebutton_copy")) {
                try {
                    ClipboardManager cm = (ClipboardManager)context.GetSystemService(Context.ClipboardService);
                    // 将文本内容放到系统剪贴板里。
                    cm.Text = umWeb.ToUrl();
                    Toast.MakeText(context, "已复制链接到剪贴板", ToastLength.Short).Show();
                }
                catch (System.Exception ex) {
                    Toast.MakeText(context, "很抱歉，浏览器打开失败", ToastLength.Short).Show();
                }
            }
            else if (snsPlatform.MShowWord.Equals("umeng_sharebutton_copyurl")) {
                try {
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(umWeb.ToUrl()));
                    context.StartActivity(intent);
                }
                catch (System.Exception ex) {
                    Toast.MakeText(context, "很抱歉，浏览器打开失败", ToastLength.Short).Show();
                }
            }
            else {
                new ShareAction(context).WithMedia(umWeb)
            .SetPlatform(media)
            .SetCallback(this)
            .Share();
            }
        }
        public void Open(string url, string title, object icon = null) {
            umWeb = new UMWeb(url);
            umWeb.Title = title;
            if (icon == null) {
                umWeb.SetThumb(new UMImage(context, Resource.Mipmap.ic_launcher));
            }
            else {
                umWeb.SetThumb(new UMImage(context, icon.ToString()));
            }
            shareAction.Open();
        }
        public void Close() {
            shareAction.Close();
        }
        public void OnCancel(SHARE_MEDIA platform) {
            Toast.MakeText(context, "分享取消了", ToastLength.Short).Show();
        }

        public void OnError(SHARE_MEDIA platform, Throwable p1) {
            Toast.MakeText(context, "分享失败了", ToastLength.Short).Show();
        }

        public void OnResult(SHARE_MEDIA platform) {
            if (platform == SHARE_MEDIA.WeixinFavorite) {
                Toast.MakeText(context, "收藏成功", ToastLength.Short).Show();
            }
            else {
                Toast.MakeText(context, "分享成功", ToastLength.Short).Show();
            }
        }

        public void OnStart(SHARE_MEDIA platform) {
        }
    }
}