
using Android.App;
using Android.Widget;
using Com.Umeng.Socialize;
using Com.Umeng.Socialize.Bean;
using Com.Umeng.Socialize.Media;
using Com.Umeng.Socialize.Shareboard;
using Com.Umeng.Socialize.Utils;
using Java.Lang;

namespace XamCnblogs.Droid.Helpers
{
    public class SharesWidget : Java.Lang.Object, IShareBoardlistener, IUMShareListener
    {
        private Activity context;
        private ShareAction shareAction;
        private UMWeb umWeb;
        public SharesWidget(Activity context)
        {
            this.context = context;

            shareAction = new ShareAction(context)
                                .SetDisplayList(SHARE_MEDIA.Weixin, SHARE_MEDIA.WeixinCircle, SHARE_MEDIA.WeixinFavorite, SHARE_MEDIA.Sina)
                                .SetShareboardclickCallback(this);
        }

        public void Onclick(SnsPlatform snsPlatform, SHARE_MEDIA media)
        {
            new ShareAction(context).WithMedia(umWeb)
                .SetPlatform(media)
                .SetCallback(this)
                .Share();
        }
        public void Open(string url, string title, object icon = null)
        {
            umWeb = new UMWeb(url);
            umWeb.Title = title;
            if (icon == null)
            {
                umWeb.SetThumb(new UMImage(context, Resource.Drawable.ic_launcher));
            }
            else
            {
                umWeb.SetThumb(new UMImage(context, icon.ToString()));
            }
            shareAction.Open();
        }
        public void Close()
        {
            shareAction.Close();
        }
        public void OnCancel(SHARE_MEDIA platform)
        {
            Toast.MakeText(context, "分享取消了", ToastLength.Short).Show();
        }

        public void OnError(SHARE_MEDIA platform, Throwable p1)
        {
            Toast.MakeText(context, "分享失败了", ToastLength.Short).Show();
        }

        public void OnResult(SHARE_MEDIA platform)
        {
            if (platform == SHARE_MEDIA.WeixinFavorite)
            {
                Toast.MakeText(context, "收藏成功", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(context, "分享成功", ToastLength.Short).Show();
            }
        }

        public void OnStart(SHARE_MEDIA platform)
        {
        }
    }
}