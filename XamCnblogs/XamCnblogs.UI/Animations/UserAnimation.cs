using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamCnblogs.UI.Animations
{
    public class UserAnimation : FadeBackgroundAnimation
    {
        private double _defaultTranslationY;

        public UserAnimation()
        {
            DurationIn = DurationOut = 10;
            EasingIn = Easing.CubicOut;
            EasingOut = Easing.CubicIn;
        }

        public override void Preparing(View content, PopupPage page)
        {
            base.Preparing(content, page);

            page.IsVisible = false;

            if (content == null) return;

            _defaultTranslationY = content.TranslationY;
        }

        public override void Disposing(View content, PopupPage page)
        {
            base.Disposing(content, page);

            page.IsVisible = true;

            if (content == null) return;

            content.TranslationY = _defaultTranslationY;
        }

        public async override Task Appearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Appearing(content, page));

            if (content != null)
            {
                var topOffset = GetTopOffset(content, page);
                content.TranslationY = topOffset;

                taskList.Add(content.TranslateTo(content.TranslationX, _defaultTranslationY, DurationIn, EasingIn));
            };

            page.IsVisible = true;

            await Task.WhenAll(taskList);
        }

        public async override Task Disappearing(View content, PopupPage page)
        {
            var taskList = new List<Task>();

            taskList.Add(base.Disappearing(content, page));

            if (content != null)
            {
                _defaultTranslationY = content.TranslationX;

                var topOffset = GetTopOffset(content, page);

                taskList.Add(content.TranslateTo(content.TranslationX, topOffset, DurationOut, EasingOut));
            };

            await Task.WhenAll(taskList);
        }
    }
}
