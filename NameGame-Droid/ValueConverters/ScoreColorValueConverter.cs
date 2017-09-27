// This converter converts a float into a color:
// Green for values greater than or equal to 50 and
// Red for values less 50

using System;
using Android.Graphics;
using MvvmCross.Platform.Converters;

namespace WillowTree.NameGame.Droid.ValueConverters
{
	public class ScoreColorValueConverter : MvxValueConverter<float, Color>
	{
		private static readonly Color LowColor = new Color(0xFF, 0x00, 0x00);
		private static readonly Color HighColor = new Color(0x00, 0xFF, 0x00);

		protected override Color Convert(float value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value < 50.00 ? LowColor : HighColor;
		}
	}
}