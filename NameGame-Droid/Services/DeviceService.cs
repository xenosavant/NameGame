// This implementation of the IDeviceService retrieves the screen width
// and height for the current orientation from the display metrics object

using System;
using WillowTree.NameGame.Core.Services;
using Xamarin.Android;
using Android.Preferences;
using Android.Util;
using Android.Content.Res;
using Android.Hardware;

namespace WillowTree.NameGame.Droid.Services
{
    public class DeviceService : IDeviceService
    {
        DisplayMetrics metrics = Resources.System.DisplayMetrics;

        public int GetScreenWidth()
        {
			return (int)metrics.WidthPixels;
        }

		public int GetScreenHeight()
		{
			return (int)metrics.HeightPixels;
		}

    }
}
