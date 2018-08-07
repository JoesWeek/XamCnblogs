using Xamarin.Forms;

namespace XamCnblogs.UI.Pages.Android
{
    public class RootPage : MultiPage<Page>
    {
        public RootPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
        }
        protected override Page CreateDefault(object item)
        {
            var page = new Page();
            if (item != null)
                page.Title = item.ToString();

            return page;
        }
        public static readonly BindableProperty SelectedSearchProperty = BindableProperty.Create(propertyName: nameof(SelectedSearch),
                returnType: typeof(bool),
                declaringType: typeof(RootPage),
                defaultValue: false);
        public bool SelectedSearch
        {
            get { return (bool)GetValue(SelectedSearchProperty); }
            set { SetValue(SelectedSearchProperty, value); }
        }

        public static readonly BindableProperty HasSearchBarProperty = BindableProperty.Create(propertyName: nameof(HasSearchBar),
                returnType: typeof(bool),
                declaringType: typeof(RootPage),
                defaultValue: true);
        public bool HasSearchBar
        {
            get { return (bool)GetValue(HasSearchBarProperty); }
            set { SetValue(HasSearchBarProperty, value); }
        }
    }
}
