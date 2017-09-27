// This converter converts a boolean value into an Android view state,
// returning a Gone state when true

using System;
using Android.Graphics;
using MvvmCross.Platform.Converters;
using Android.Views;

namespace WillowTree.NameGame.Droid.ValueConverters
{
    public class HiddenValueConverter : MvxValueConverter<bool, Android.Views.ViewStates>
    {
        protected override Android.Views.ViewStates Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ? Android.Views.ViewStates.Gone : Android.Views.ViewStates.Visible;
        }
    }
}
    