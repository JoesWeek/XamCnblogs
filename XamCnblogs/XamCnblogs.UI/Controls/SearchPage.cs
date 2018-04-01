using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
	public class SearchPage : ContentPage
    {
        public static readonly BindableProperty SearchPlaceHolderTextProperty =
            BindableProperty.Create(nameof(SearchPlaceHolderText),
                typeof(string),
                typeof(SearchPage),
                string.Empty);

        public static readonly BindableProperty SearchTextProperty =
            BindableProperty.Create(nameof(SearchText),
                typeof(string),
                typeof(SearchPage),
                string.Empty);


        public static readonly BindableProperty SearchCommandProperty =
            BindableProperty.Create(nameof(SearchCommand),
                typeof(ICommand),
                typeof(SearchPage));

        public string SearchPlaceHolderText
        {
            get
            {
                return (string)GetValue(SearchPlaceHolderTextProperty);
            }
            set
            {
                SetValue(SearchPlaceHolderTextProperty, value);
            }
        }

        public string SearchText
        {
            get
            {
                return (string)GetValue(SearchTextProperty);
            }
            set
            {
                SetValue(SearchTextProperty, value);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return (ICommand)GetValue(SearchCommandProperty);
            }
            set
            {
                SetValue(SearchCommandProperty, value);
            }
        }
    }
}