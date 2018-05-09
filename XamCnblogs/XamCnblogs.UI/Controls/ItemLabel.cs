using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamCnblogs.UI.Controls
{
    public class ItemLabel : Label
    {
        public double LineSpacing { get; set; }
        public int MaxLines { get; set; }

        public static readonly BindableProperty IsLuckyProperty = BindableProperty.Create(propertyName: nameof(IsLucky),
                returnType: typeof(bool),
                declaringType: typeof(ItemLabel),
                defaultValue: false);
        public bool IsLucky
        {
            get { return (bool)GetValue(IsLuckyProperty); }
            set { SetValue(IsLuckyProperty, value); }
        }
    }
}