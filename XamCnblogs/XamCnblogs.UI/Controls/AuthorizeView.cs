using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class AuthorizeView : ContentView
    {
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(propertyName: nameof(Source),
                returnType: typeof(string),
                declaringType: typeof(AuthorizeView),
                defaultValue: default(string));
        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public void OnAuthorizeStarted(AuthorizeStartedEventArgs e)
        {
            AuthorizeStarted?.Invoke(this, e);
        }

        public event AuthorizeStartedEventHandler AuthorizeStarted;
    }
    public class AuthorizeStartedEventArgs : EventArgs
    {
        public string Code { get; set; }
    }
    public delegate void AuthorizeStartedEventHandler(object sender, AuthorizeStartedEventArgs e);
}
