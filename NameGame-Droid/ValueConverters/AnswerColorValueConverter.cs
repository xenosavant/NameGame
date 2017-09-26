using System;
using Android.Graphics;
using MvvmCross.Platform.Converters;


namespace WillowTree.NameGame.Droid.ValueConverters
{
	public class AnswerColorValueConverter : MvxValueConverter<bool, Color>
	{
		private static readonly Color CorrectChoiceColor = new Color(0x00, 0xFF, 0x00, 0x88);
		private static readonly Color IncorrectChoiceColor = new Color(0xFF, 0x00, 0x00, 0x88);

		protected override Color Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value ? CorrectChoiceColor : IncorrectChoiceColor;
		}
	}
}
