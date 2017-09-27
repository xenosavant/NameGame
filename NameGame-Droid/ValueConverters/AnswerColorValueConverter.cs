// This converter converts a bool into a color:
// 50% Transparent green for correct and
// 50% Transparent red for incorrect

using System;
using Android.Graphics;
using MvvmCross.Platform.Converters;

namespace WillowTree.NameGame.Droid.ValueConverters
{
    public class AnswerColorValueConverter : MvxValueConverter<bool, Color>
    {
        static readonly Color CorrectChoiceColor = new Color(0x00, 0xFF, 0x00, 0x88);
        static readonly Color IncorrectChoiceColor = new Color(0xFF, 0x00, 0x00, 0x88);

        protected override Color Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ? CorrectChoiceColor : IncorrectChoiceColor;
        }
    }
}
