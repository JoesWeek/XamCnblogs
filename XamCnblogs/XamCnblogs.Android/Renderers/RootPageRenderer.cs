using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Java.Lang;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamCnblogs.Droid.Renderers;
using XamCnblogs.UI.Controls;
using XamCnblogs.UI.Pages.Account;
using XamCnblogs.UI.Pages.Android;
using XamCnblogs.UI.Pages.Search;

[assembly: ExportRenderer(typeof(RootPage), typeof(RootPageRenderer))]

namespace XamCnblogs.Droid.Renderers
{
    public class RootPageRenderer : VisualElementRenderer<RootPage>, Android.Views.View.IOnFocusChangeListener, Android.Views.View.IOnClickListener, Android.Text.ITextWatcher, TextView.IOnEditorActionListener
    {
        bool _disposed;
        IPageController PageController => Element as IPageController;

        FragmentManager _fragmentManager;
        FragmentManager FragmentManager => _fragmentManager ?? (_fragmentManager = ((FormsAppCompatActivity)Context).SupportFragmentManager);

        LinearLayout rootView;
        FrameLayout searchContainer;
        FrameLayout rootContainer;

        Fragment barFragment;
        Fragment searchFragment;

        HomeTabbedPage barPage;
        SearchPage searchPage;

        EditText searchEditContent;
        ImageButton searchBackButton;
        ImageButton searchCloseButton;

        public RootPageRenderer(Context context) : base(context)
        {
            AutoPackage = false;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<RootPage> e)
        {
            base.OnElementChanged(e);
            var activity = (FormsAppCompatActivity)Context;

            if (e.NewElement != null)
            {
                foreach (var item in Element.Children)
                {
                    if (item is HomeTabbedPage)
                    {
                        barPage = item as HomeTabbedPage;
                        barPage.CurrentPageChanged += delegate (object sender, System.EventArgs ev)
                        {
                            if (barPage.CurrentPage is AccountPage)
                            {
                                Element.HasSearchBar = false;
                            }
                            else
                            {
                                Element.HasSearchBar = true;
                            }
                        };
                    }
                    if (item is SearchPage)
                    {
                        searchPage = item as SearchPage;
                    }
                }
                rootView = activity.LayoutInflater.Inflate(Resource.Layout.RootView, null) as LinearLayout;
                searchContainer = rootView.FindViewById<FrameLayout>(Resource.Id.search_view_container);

                searchEditContent = rootView.FindViewById<EditText>(Resource.Id.search_edit_content);
                searchEditContent.OnFocusChangeListener = this;
                searchEditContent.AddTextChangedListener(this);
                searchEditContent.SetOnEditorActionListener(this);

                searchBackButton = rootView.FindViewById<ImageButton>(Resource.Id.search_back_button);
                searchBackButton.SetOnClickListener(this);

                searchCloseButton = rootView.FindViewById<ImageButton>(Resource.Id.search_close_button);
                searchCloseButton.SetOnClickListener(this);

                rootContainer = rootView.FindViewById<FrameLayout>(Resource.Id.root_container);
                rootContainer.Id = rootContainer.GetHashCode();

                ViewGroup.AddView(rootView);

                SwitchBarPage();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(this.Element.SelectedSearch))
            {
                if (this.Element.SelectedSearch)
                {
                    HideBarPage();
                    SwitchSearchPage();

                    StartSearch();
                }
                else
                {
                    EndSearch();

                    HideSearchPage();
                    SwitchBarPage();
                }
            }
            else if (e.PropertyName == nameof(this.Element.HasSearchBar))
            {
                if (Element.HasSearchBar)
                {
                    searchContainer.Visibility = ViewStates.Visible;
                }
                else
                {
                    searchContainer.Visibility = ViewStates.Gone;
                }
                this.UpdateLayout();
            }
        }
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            Context context = Context;
            int width = r - l;
            int height = b - t;

            rootView.Measure(AppCompat.MeasureSpecFactory.MakeMeasureSpec(width, MeasureSpecMode.AtMost), AppCompat.MeasureSpecFactory.MakeMeasureSpec(height, MeasureSpecMode.AtMost));

            if (width > 0 && height > 0)
            {
                if (Element.HasSearchBar)
                {
                    PageController.ContainerArea = new Rectangle(0, 0, context.FromPixels(width), context.FromPixels(height - searchContainer.MeasuredHeight));
                }
                else
                {
                    PageController.ContainerArea = new Rectangle(0, 0, context.FromPixels(width), context.FromPixels(height));
                }
                rootView.Layout(0, 0, width, b);
            }

            base.OnLayout(changed, l, t, r, b);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                RemoveAllViews();
                foreach (var pageToRemove in Element.Children)
                {
                    IVisualElementRenderer pageRenderer = Platform.GetRenderer(pageToRemove);
                    if (pageRenderer != null)
                    {
                        pageRenderer.View.RemoveFromParent();
                        pageRenderer.Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            PageController.SendAppearing();
        }
        protected override void OnDetachedFromWindow()
        {
            base.OnDetachedFromWindow();
            PageController.SendDisappearing();
        }

        void SwitchBarPage()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            if (barFragment == null)
            {
                barFragment = AppCompat.FragmentContainer.CreateInstance(barPage);
                transaction.Add(rootContainer.Id, barFragment).CommitAllowingStateLoss();
            }
            else
            {
                transaction.Show(barFragment).CommitAllowingStateLoss();
            }
        }
        void SwitchSearchPage()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            if (searchFragment == null)
            {
                searchFragment = AppCompat.FragmentContainer.CreateInstance(searchPage);
                transaction.Add(rootContainer.Id, searchFragment).CommitAllowingStateLoss();
            }
            else
            {
                transaction.Show(searchFragment).CommitAllowingStateLoss();
            }
        }
        public void HideBarPage()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            if (barFragment != null)
            {
                transaction.Hide(barFragment).CommitAllowingStateLoss();
            }
        }
        public void HideSearchPage()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            if (searchFragment != null)
            {
                transaction.Hide(searchFragment).CommitAllowingStateLoss();
            }
        }

        public void OnFocusChange(Android.Views.View view, bool hasFocus)
        {
            var page = this.Element as RootPage;
            if (hasFocus && !page.SelectedSearch)
            {
                page.SelectedSearch = true;
            }
        }
        void StartSearch()
        {
            searchBackButton.Clickable = true;
            searchBackButton.SetImageResource(Resource.Drawable.toolbar_back);

            searchCloseButton.Visibility = ViewStates.Visible;
        }
        void EndSearch()
        {
            searchBackButton.Clickable = false;
            searchBackButton.SetImageResource(Resource.Drawable.toolbar_search);

            searchCloseButton.Visibility = ViewStates.Gone;

            searchEditContent.ClearFocus();
            searchEditContent.Text = "";
        }

        public void OnClick(Android.Views.View view)
        {
            if (view == searchBackButton || view == searchCloseButton)
            {
                var page = this.Element as RootPage;
                page.SelectedSearch = false;
            }
        }

        public void AfterTextChanged(IEditable s)
        {
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            var page = this.Element as RootPage;

            searchPage.OnSearchChanged(new SearchChangedEventArgs
            {
                Value = s.ToString()
            });
        }

        public bool OnEditorAction(TextView v, [GeneratedEnum] ImeAction actionId, KeyEvent e)
        {
            if (actionId == ImeAction.Search)
            {
                searchPage.OnSearchChanged(new SearchChangedEventArgs
                {
                    Value = v.Text
                });
                return true;
            }
            return false;
        }
    }
}